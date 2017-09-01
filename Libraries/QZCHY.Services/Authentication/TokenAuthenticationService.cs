using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Services.AccountUsers;
using System;
using System.Security.Claims;
using System.Web;

namespace QZCHY.Services.Authentication
{
    public class TokenAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IAccountUserService _customerService;
        private readonly AccountUserSettings _customerSettings;

        private AccountUser _cachedAccountUser;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">AccountUser service</param>
        /// <param name="customerSettings">AccountUser settings</param>
        public TokenAuthenticationService(HttpContextBase httpContext,
            IAccountUserService customerService, AccountUserSettings customerSettings)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._customerSettings = customerSettings;
        }

        public void SignOut()
        {
            System.Web.HttpContext.Current.User = null;
        }

        /// <summary>
        /// 获取已认证的用户
        /// </summary>
        /// <returns></returns>
        public virtual AccountUser GetAuthenticatedAccountUser()
        {
            if (_cachedAccountUser != null)
                return _cachedAccountUser;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated)
            {
                return null;
            }

            var principal = HttpContext.Current.User as ClaimsPrincipal;
            var guidClaim = principal.FindFirst(ct => ct.Type == ClaimTypes.NameIdentifier);
            if (guidClaim == null) return null;

            var property = _customerService.GetAccountUserByGuid(new Guid(guidClaim.Value));
            if (property != null && property.Active && !property.Deleted && property.IsRegistered())
                _cachedAccountUser = property;
            return _cachedAccountUser;
        }
    }
}
