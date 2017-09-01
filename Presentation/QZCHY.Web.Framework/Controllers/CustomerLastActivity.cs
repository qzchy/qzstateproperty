using QZCHY.Core;
using QZCHY.Core.Infrastructure;
using QZCHY.Services.AccountUsers;
using System;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace QZCHY.Web.Framework.Controllers
{
    public class CustomerLastActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null || actionContext.Request == null || actionContext.Request.Method.Method== null)
                return;

            //only GET requests
            if (!String.Equals(actionContext.Request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var customer = workContext.CurrentAccountUser;

            if (customer == null) return;

            //update last activity date
            if (!customer.LastActivityDate.HasValue || customer.LastActivityDate.Value.AddMinutes(1.0) < DateTime.Now)
            {
                var customerService = EngineContext.Current.Resolve<IAccountUserService>();
                customer.LastActivityDate = DateTime.Now;
                customerService.UpdateAccountUser(customer);
            }

        }
    }
}