using QZCHY.Core;
using QZCHY.Core.Infrastructure;
using QZCHY.Web.Framework.Security;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace QZCHY.Web.Framework.Controllers
{
    //[QMHttpsRequirement(SslRequirement.Yes)]
    [AdminValidateIpAddress]
    [QMAdminAuthorize]
    public class BaseAdminApiController : BaseApiController
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            //set work context to admin mode
            EngineContext.Current.Resolve<IWorkContext>().IsAdmin = true;
            base.Initialize(controllerContext);
        }
    }

    /// <summary>
    /// 后台管理API 角色验证
    /// </summary>
    public class QMAdminAuthorizeAttribute : AuthorizeAttribute
    {
        public QMAdminAuthorizeAttribute()
        {
            Roles = Core.Domain.AccountUsers.SystemAccountUserRoleNames.Registered;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }
    }
}
