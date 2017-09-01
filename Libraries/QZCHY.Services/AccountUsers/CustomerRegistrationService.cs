using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Services.Security;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace QZCHY.Services.AccountUsers
{
    public class AccountUserRegistrationService : IAccountUserRegistrationService
    {
        private readonly IAccountUserService _customerService;
        private readonly IEncryptionService _encryptionService;

        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly AccountUserSettings _customerSettings;

        public AccountUserRegistrationService(IAccountUserService customerService, IEncryptionService encryptionService,
             RewardPointsSettings rewardPointsSettings,
        AccountUserSettings customerSettings)
        {
            _customerService = customerService;
            _encryptionService = encryptionService;
            _rewardPointsSettings = rewardPointsSettings;
            _customerSettings = customerSettings;
        }

        /// <summary>
        /// 验证用户的正确性
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<AccountUser> ValidateAccountUserAsync(string account, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("密码不能为空");

            var customer = _customerService.GetAccountUserByAccount(account);
            if (customer != null)
            {
                bool passwordCorrect = false;
                switch (customer.PasswordFormat)
                {
                    case PasswordFormat.Clear:
                        {
                            passwordCorrect = password == customer.Password;
                        }
                        break;
                    case PasswordFormat.Encrypted:
                        {
                            passwordCorrect = _encryptionService.EncryptText(password) == customer.Password;
                        }
                        break;
                    case PasswordFormat.Hashed:
                        {
                            string saltKey = _encryptionService.CreateSaltKey(5);
                            passwordCorrect = _encryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat) == customer.Password;
                        }
                        break;
                    default:
                        break;
                }

                if (passwordCorrect) return Task.FromResult(customer);
            }

            return Task.FromResult<AccountUser>(null); ;
        }

        /// <summary>
        /// 验证用户的正确性
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<AccountUser> ValidateWechatAppAccountUserAsync(string code)
        {
            // appid:"wxf1fe2ec4f67ca88d",
            // secret:"89d07bfc96a63e8f4e56bbeeb76bc829",
            //code: res.code,
            // grant_type:"authorization_code"
            var authUrl = "";// ConfigurationManager.AppSettings["url"];
            var appid= "";// ConfigurationManager.AppSettings["appid"];
            var secret = "";//  ConfigurationManager.AppSettings["secret"];

            string url = "{0}?appid={1}&secret={2}&js_code={3}&grant_type=authorization_code";
            HttpRequestMessage hrm = new HttpRequestMessage();
            hrm.Method = HttpMethod.Get;
            var responese = hrm.CreateResponse(url);
            var result = responese.Content.ToString();

            return Task.FromResult<AccountUser>(null); ;


            //    //设置HttpClientHandler的AutomaticDecompression

            //    var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };


            //    //创建HttpClient（注意传入HttpClientHandler）

            //    using (var http = new HttpClient())

            //    {

            //        //使用FormUrlEncodedContent做HttpContent

            //        var content = new FormUrlEncodedContent(new Dictionary<string, string>()

            //{

            //    {"id", userId},

            //    {"force_gzip", "1"}

            //});

            //        //await异步等待回应

            //        var response = await http.PostAsync(url, content);

            //        //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）

            //        Console.WriteLine(await response.Content.ReadAsStringAsync());

            //    }
        }


        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AccountUserLoginResults ValidateAccountUser(string account, string password)
        {
            var customer = _customerService.GetAccountUserByAccount(account);

            if (customer == null)
                return AccountUserLoginResults.AccountUserNotExist;
            if (customer.Deleted)
                return AccountUserLoginResults.Deleted;
            if (!customer.Active)
                return AccountUserLoginResults.NotActive;
            //only registered can login
            if (!customer.IsRegistered())
                return AccountUserLoginResults.NotRegistered;

            string pwd = "";
            switch (customer.PasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    pwd = _encryptionService.EncryptText(password);
                    break;
                case PasswordFormat.Hashed:
                    pwd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt, _customerSettings.HashedPasswordFormat);
                    break;
                default:
                    pwd = password;
                    break;
            }

            bool isValid = pwd == customer.Password;
            if (!isValid)
                return AccountUserLoginResults.WrongPassword;

            //save last login date
            customer.LastLoginDate = DateTime.Now;
            _customerService.UpdateAccountUser(customer);
            return AccountUserLoginResults.Successful;

        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual AccountUserRegistrationResult RegisterAccountUser(AccountUserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.AccountUser == null)
                throw new ArgumentNullException("无法加载当前用户");

            var result = new AccountUserRegistrationResult();
            if (request.AccountUser.IsSearchEngineAccount())
            {
                result.AddError("搜索引擎用户无法注册");
                return result;
            }
            if (request.AccountUser.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }

            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("密码不能为空");
                return result;
            }

            //暂时
            if (_customerSettings.UsernamesEnabled)
            {
                if (String.IsNullOrEmpty(request.Username))
                {
                    result.AddError("用户名不能为空");
                    return result;
                }
            }

            if (_customerService.GetAccountUserByUsername(request.Username) != null)
            {
                result.AddError(string.Format("用户名已经注册用户"));
                return result;
            }

            //at this point request is valid
            request.AccountUser.UserName = request.Username; 
            request.AccountUser.PasswordFormat = request.PasswordFormat;

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        request.AccountUser.Password = request.Password;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        request.AccountUser.Password = _encryptionService.EncryptText(request.Password);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        request.AccountUser.PasswordSalt = saltKey;
                        request.AccountUser.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _customerSettings.HashedPasswordFormat);
                    }
                    break;
                default:
                    break;
            }

            request.AccountUser.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _customerService.GetAccountUserRoleBySystemName(SystemAccountUserRoleNames.Registered);
            if (registeredRole == null)
                throw new QZCHYException("'Registered' role could not be loaded");
            request.AccountUser.AccountUserRoles.Add(registeredRole);
            //remove from 'Guests' role
            var guestRole = request.AccountUser.AccountUserRoles.FirstOrDefault(cr => cr.SystemName == SystemAccountUserRoleNames.Guests);
            if (guestRole != null)
                request.AccountUser.AccountUserRoles.Remove(guestRole);

            //Add reward points for customer registration (if enabled)
            if (_rewardPointsSettings.Enabled &&
                _rewardPointsSettings.PointsForRegistration > 0)
            {
                //TODO：注册获得积分
                // request.AccountUser.AddRewardPointsHistoryEntry(_rewardPointsSettings.PointsForRegistration, "注册获得积分");
            }

            _customerService.UpdateAccountUser(request.AccountUser);
            return result;
        }


    }
}
