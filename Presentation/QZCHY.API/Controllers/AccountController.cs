using QZCHY.Core;
using QZCHY.Services.Authentication;
using QZCHY.Services.Common;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Messages;
using QZCHY.Web.Framework.Controllers;
using System.Web.Http;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Services.Logging;
using System;
using QZCHY.Web.Framework.Response;
using QZCHY.API.Models.AccountUsers;
using QZCHY.Web.Api.Extensions;
using System.Linq;
using QZCHY.Services.Property;

namespace QZCHY.API.Controllers
{
    [RoutePrefix("Systemmanage/Accounts")]
    public class AccountController : BaseAdminApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountUserService _accountService;
        private readonly IAccountUserRegistrationService _accountUserRegistrationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IAccountUserActivityService _accountUserActivityService;
        private readonly IGovernmentService _governmentService;

        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly AccountUserSettings _accountUserSettings;

        public AccountController(
            IAuthenticationService authenticationService,
            IAccountUserService customerService,
                 IAccountUserRegistrationService customerRegistrationService,
             IGenericAttributeService genericAttributeService,
            IWorkflowMessageService workflowMessageService,
            IAccountUserActivityService accountUserActivityService,
            IGovernmentService governmentService,
             IWebHelper webHelper,
            IWorkContext workContext,
            AccountUserSettings customerSettings)
        {
            _authenticationService = authenticationService;
            _accountService = customerService;
            _accountUserRegistrationService = customerRegistrationService;
            _genericAttributeService = genericAttributeService;
            _workflowMessageService = workflowMessageService;
            _accountUserActivityService = accountUserActivityService;
            _governmentService = governmentService;

            _webHelper = webHelper;
            _workContext = workContext;
            _accountUserSettings = customerSettings;
        }

        #region 用户API

        [HttpGet]
        [Route("Unique/{name}")]
        public IHttpActionResult UniqueCheck(string name)
        {
            var result = !_accountService.NameUniqueCheck(name);

            return Ok(result);
        }

        [HttpGet]
        [Route("{accountId:int}")]
        public IHttpActionResult Get(int accountId)
        {
            var account = _accountService.GetAccountUserById(accountId);
            if (account == null || account.Deleted)
                return NotFound();


            var model = account.ToModel();
            model.IsAdministrator = account.IsAdmin();

            //activity log
            _accountUserActivityService.InsertActivity("GetAccountInfo", "获取 名为 {0} 的用户信息", account.UserName);

            return Ok(model);         
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(string query = "", string sort = "", int pageSize = Int32.MaxValue, int pageIndex = 0, bool showHidden = false)
        {
            //初始化排序条件
            var sortConditions = PropertySortCondition.Instance(sort);

            //特殊字段排序调整
            if (sort.ToLower().StartsWith("governmentname")) sortConditions[0].PropertyName = "Government";


            var accounts = _accountService.GetAllAccountUsers(query, pageIndex, pageSize, showHidden, sortConditions);

            var response = new ListResponse<AccountUserModel>
            {
                Paging = new Paging
                {
                    PageIndex = pageIndex,
                    PageSize = pageIndex,
                    Total = accounts.TotalCount,
                    FilterCount = string.IsNullOrEmpty(query) ? accounts.TotalCount : accounts.Count,
                },
                Data = accounts.Select(s =>
                {
                    var accountModel = s.ToModel();
                    accountModel.GovernmentName = s.Government.Name;
                    return accountModel;
                })
            };

            //activity log
            _accountUserActivityService.InsertActivity("GetAccountList", "获取用户列表信息");

            return Ok(response);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="accountModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(AccountUserModel accountModel)
        {
            var account = accountModel.ToEntity();

            //问题： An entity object cannot be referenced by multiple instances of IEntityChangeTracker 
            //状态：Fixed
            //原因：不明，但是应该和缓存机制有关
            var government = _governmentService.GetGovernmentUnitById(accountModel.GovernmentId);
            if (government == null) return BadRequest("用户所属单位不存在");
            account.Government = government;

            var registerRole = _accountService.GetAccountUserRoleBySystemName(SystemAccountUserRoleNames.Registered);
            var adminRole = _accountService.GetAccountUserRoleBySystemName(SystemAccountUserRoleNames.Administrators);

            account.Password = "123456";  //设置初始密码

            var role = _accountService.GetAccountUserRoleBySystemName(accountModel.RoleName);
            if (role != null && accountModel.RoleName != SystemAccountUserRoleNames.Registered) account.AccountUserRoles.Add(role);
       
            var registrationRequest = new AccountUserRegistrationRequest(account, account.UserName,
    account.Password, _accountUserSettings.DefaultPasswordFormat, accountModel.Active);

            var registrationResult = _accountUserRegistrationService.RegisterAccountUser(registrationRequest);
            if (registrationResult.Success)
            {
                //保存用户
                _accountService.InsertAccountUser(account);

                //activity log
                _accountUserActivityService.InsertActivity("AddNewAccount", "增加 名为 {0} 的用户", account.UserName);

                return Ok(account.ToModel());
            }
            else return BadRequest("添加用户失败");
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="accountModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{accountId:int}")]
        public IHttpActionResult UpdateAccount(int accountId, AccountUserModel accountModel)
        {
            var account = _accountService.GetAccountUserById(accountId);
            if (account == null || account.Deleted) return NotFound();
            account = accountModel.ToEntity(account);

            var registerRole = _accountService.GetAccountUserRoleBySystemName(SystemAccountUserRoleNames.Registered);

            account.AccountUserRoles.Clear();

            var role = _accountService.GetAccountUserRoleBySystemName(accountModel.RoleName);
            if (role != null && accountModel.RoleName != SystemAccountUserRoleNames.Registered) account.AccountUserRoles.Add(role);
            account.AccountUserRoles.Add(registerRole);

            if (accountModel.InitPassword) account.Password = "123456";  //设置初始密码

            //单位更新
            if (accountModel.GovernmentId != account.Government.Id)
            {
                var governament = _governmentService.GetGovernmentUnitById(accountModel.GovernmentId);
                if (governament == null) return BadRequest("用户所属单位不存在");
                account.Government = governament;
            }

            //保存用户
            _accountService.UpdateAccountUser(account);

            //activity log
            _accountUserActivityService.InsertActivity("UpdateAccount", "更新 名为 {0} 的用户的基本信息", account.UserName);

            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

            return Ok(account.ToModel());
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("resetpwd")]
        public IHttpActionResult ResetPwd(AccountResetPwdModel accountResetPwdModel)
        {

            var account = _accountService.GetAccountUserByUsername(accountResetPwdModel.UserName);

            if (account.Password != accountResetPwdModel.Password) return BadRequest("原密码输入不正确！");

            if (accountResetPwdModel.Newpassword != accountResetPwdModel.Againpassword) return BadRequest("两次输入的密码不一致！");

            account.Password = accountResetPwdModel.Newpassword;

            //保存用户
            _accountService.UpdateAccountUser(account);

            //activity log
            _accountUserActivityService.InsertActivity("UpdateAccount", "更新 名为 {0} 的用户的基本信息", account.UserName);

            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

            return Ok();
        }

        [HttpDelete]
        [Route("{accountId:int}")]
        public IHttpActionResult DeleteAccount(int accountId)
        {

            var account = _accountService.GetAccountUserById(accountId);
            if (account == null || account.Deleted) return NotFound();

            _accountService.DeleteAccountUser(account);

            //activity log
            _accountUserActivityService.InsertActivity("DeleteAccount", "删除 名为 {0} 的用户", account.UserName);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="accountIdString"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{accountIdString}")]
        public IHttpActionResult DeleteAccount(string accountIdString)
        {
            var idStringArr = accountIdString.Split('_');
            foreach (var idStr in idStringArr)
            {
                int id = 0;
                if (!int.TryParse(idStr, out id)) continue;

                var account = _accountService.GetAccountUserById(id);
                if (account == null) continue;

                _accountService.DeleteAccountUser(account);
            }


            //活动日志
            _accountUserActivityService.InsertActivity("DeleteAccounts", "批量删除 Id为 {0} 的用户", accountIdString);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }
        #endregion

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("Register")]
        //public IHttpActionResult Register(RegisterModel model)
        //{
        //    //判断当前是否为认证用户
        //    if (ControllerContext.RequestContext.Principal.Identity.IsAuthenticated)
        //        return BadRequest("当前用户已经注册");

        //    //检查是否允许注册用户
        //    if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
        //    {
        //        return BadRequest("用户注册已关闭");
        //    }

        //    if (_workContext.CurrentCustomer.IsRegistered()) return BadRequest("当前用户已注册");

        //    var accountUser = _workContext.CurrentCustomer;
        //    if (accountUser.IsRegistered()) return BadRequest("当前用户已经注册");

        //    //TODO：自定义属性

        //    //TODO：验证码

        //    if (_customerSettings.UsernamesEnabled && model.Username != "")
        //    {
        //        model.Username = model.Username.Trim();
        //    }

        //    bool isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
        //    var registrationRequest = new CustomerRegistrationRequest(accountUser, model.Email, model.Mobile, model.Username,
        //        model.Password, _customerSettings.DefaultPasswordFormat, isApproved);

        //    var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
        //    if (registrationResult.Success)
        //    {
        //        //associated with external account (if possible)

        //        //insert default address (if possible)

        //        //notifications
        //        //_workflowMessageService

        //        switch (_customerSettings.UserRegistrationType)
        //        {
        //            case UserRegistrationType.EmailValidation:
        //                {
        //                    //email validation message
        //                    _genericAttributeService.SaveAttribute(accountUser, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
        //                    _workflowMessageService.SendCustomerEmailValidationMessage(accountUser);

        //                    //result
        //                    return Ok(new ApiResponseResult { Code = 1, Message = "邮件认证" });
        //                }
        //            case UserRegistrationType.AdminApproval:
        //                {
        //                    return Ok(new ApiResponseResult { Code = 1, Message = "管理员认证" });
        //                }
        //            default:
        //                {
        //                    //send accountUser welcome message
        //                    _workflowMessageService.SendCustomerWelcomeMessage(accountUser);

        //                    return Ok(new ApiResponseResult { Code = 1, Message = "注册成功" });
        //                }
        //        }
        //    }

        //    //errors
        //    foreach (var error in registrationResult.Errors)
        //        ModelState.AddModelError("", error);
        //    return BadRequest(ModelState);
        //}


        #region 登陆实现
        //[HttpPost]
        //[Route("Login")]
        //public IHttpActionResult Login(LoginModel model)
        //{

        //    var loginResult = _customerRegistrationService.ValidateCustomer(model.Account, model.Password);

        //    if (loginResult == CustomerLoginResults.Successful)
        //    {
        //        var accountUser = _customerService.GetCustomerByAccount(model.Account);

        //        //migrate shopping cart
        //        //_shoppingCartService.MigrateShoppingCart(_workContext.CurrentCustomer, accountUser, true);

        //        //sign in new accountUser
        //        _authenticationService.SignIn(accountUser, model.RememberMe);

        //        //activity log
        //        //_accountUserActivityService.InsertActivity("PublicAccount.Login", _localizationService.GetResource("ActivityLog.PublicAccount.Login"), accountUser);

        //        return Ok();
        //    }
        //    else
        //    {
        //        switch (loginResult)
        //        {
        //            case CustomerLoginResults.NotActive:
        //                return BadRequest("用户未激活");
        //            default:
        //                return BadRequest("用户名不存在或密码不正确");
        //        }
        //    }
        //} 
        #endregion

        // GET: api/accountUser/5
        //[HttpGet]
        //[Route("{id:int}")]
        //[Authorize]
        //public string Get(int id)
        //{
        //    var user = System.Web.HttpContext.Current.User;
        //    return user.Identity.Name;
        //}

        // POST: api/accountUser
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/accountUser/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/accountUser/5
        public void Delete(int id)
        {
        }
    }
}