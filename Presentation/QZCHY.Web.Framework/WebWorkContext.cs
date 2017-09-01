using System;
using System.Web;
using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Fakes;
using QZCHY.Services.Helpers;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Authentication;
using QZCHY.Services.Common;

namespace QZCHY.Web.Framework
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string AccountUserCookieName = "QZCHY.customer";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IAccountUserService _customerService;
        //private readonly IvenderService _venderService;
        private readonly IAuthenticationService _authenticationService;
        //private readonly ILanguageService _languageService;
        //private readonly ICurrencyService _currencyService;
        //private readonly IGenericAttributeService _genericAttributeService;
        private readonly IUserAgentHelper _userAgentHelper;
        //private readonly IStoreMappingService _storeMappingService;

        private AccountUser _cachedUser;
        private AccountUser _originalUserIfImpersonated;
        //private vender _cachedvender;


        #endregion

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            IAccountUserService customerService,
            //IvenderService venderService,
            IAuthenticationService authenticationService,
            //ILanguageService languageService,
            //ICurrencyService currencyService,
            //IGenericAttributeService genericAttributeService,
            IUserAgentHelper userAgentHelper
            //IStoreMappingService storeMappingService
            )
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            //this._venderService = venderService;
            this._authenticationService = authenticationService;
            //this._languageService = languageService;
            //this._currencyService = currencyService;
            //this._genericAttributeService = genericAttributeService;
            this._userAgentHelper = userAgentHelper;
            //this._storeMappingService = storeMappingService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取访客Token
        /// </summary>
        /// <returns></returns>
        protected virtual string GetGuestToken()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Headers["Authorization"];
        }

        protected virtual HttpCookie GetAccountUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[AccountUserCookieName];
        }

        protected virtual void SetAccountUserCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(AccountUserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(AccountUserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual AccountUser CurrentAccountUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                AccountUser customer = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //check whether request is made by a background task
                    //in this case return built-in customer record for background task

                    customer = _customerService.GetAccountUserBySystemName(SystemAccountUserNames.BackgroundTask);
                }

                //check whether request is made by a search engine
                //in this case return built-in customer record for search engines 
                //or comment the following two lines of code in order to disable this functionality
                if (customer == null || customer.Deleted || !customer.Active)
                {
                    if (_userAgentHelper.IsSearchEngine())
                        customer = _customerService.GetAccountUserBySystemName(SystemAccountUserNames.SearchEngine);
                }

                //registered user
                if (customer == null || customer.Deleted || !customer.Active)
                {
                    customer = _authenticationService.GetAuthenticatedAccountUser();
                }

                if (customer == null || customer.Deleted || !customer.Active) return null;
                else _cachedUser = customer;

                //impersonate user if required (currently used for 'phone order' support)
                //if (customer != null && !customer.Deleted && customer.Active)
                //{
                //    var impersonatedAccountUserId = customer.GetAttribute<int?>(sy.ImpersonatedAccountUserId);
                //    if (impersonatedAccountUserId.HasValue && impersonatedAccountUserId.Value > 0)
                //    {
                //        var impersonatedAccountUser = _customerService.GetAccountUserById(impersonatedAccountUserId.Value);
                //        if (impersonatedAccountUser != null && !impersonatedAccountUser.Deleted && impersonatedAccountUser.Active)
                //        {
                //            //set impersonated customer
                //            _originalUserIfImpersonated = customer;
                //            customer = impersonatedAccountUser;
                //        }
                //    }
                //}

                //load guest customer
                //if (customer == null || customer.Deleted || !customer.Active)
                //{
                //    var customerCookie = GetAccountUserCookie();
                //    if (customerCookie != null && !String.IsNullOrEmpty(customerCookie.Value))
                //    {
                //        Guid customerGuid;
                //        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                //        {
                //            var customerByCookie = _customerService.GetAccountUserByGuid(customerGuid);
                //            if (customerByCookie != null &&
                //                //this customer (from cookie) should not be registered
                //                !customerByCookie.IsRegistered())
                //                customer = customerByCookie;
                //        }
                //    }
                //}


                //create guest if not exists
                //if (customer == null || customer.Deleted || !customer.Active)
                //{
                //    customer = _customerService.InsertGuestAccountUser();
                //}

                //validation
                //if (!customer.Deleted && customer.Active)
                //{
                //    SetAccountUserCookie(customer.AccountUserGuid);
                //    _cachedUser = customer;
                //}

                return _cachedUser;
            }
            set
            {
                SetAccountUserCookie(value.AccountUserGuid);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Gets or sets the original customer (in case the current one is impersonated)
        /// </summary>
        public virtual AccountUser OriginalUserIfImpersonated
        {
            get
            {
                return _originalUserIfImpersonated;
            }
        }

        /// <summary>
        /// Gets or sets the current vender (logged-in manager)
        /// </summary>
        //public virtual vender Currentvender
        //{
        //    get
        //    {
        //        if (_cachedvender != null)
        //            return _cachedvender;

        //        var currentAccountUser = this.CurrentAccountUser;
        //        if (currentAccountUser == null)
        //            return null;

        //        //var vender =  _venderService.GetvenderById(currentAccountUser.venderId);

        //        ////validation
        //        //if (vender != null && !vender.Deleted && vender.Active)
        //        //    _cachedvender = vender;

        //        return _cachedvender;
        //    }
        //}

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
