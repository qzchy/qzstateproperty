using QZCHY.Core;
using QZCHY.Core.Infrastructure;
using QZCHY.Services.AccountUsers;
using System;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace QZCHY.Web.Framework.Controllers
{
    public class StoreIpAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null || actionContext.Request == null || actionContext.Request.Method.Method == null)
                return;

            //only GET requests
            if (!String.Equals(actionContext.Request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();

            //update IP address
            string currentIpAddress = webHelper.GetCurrentIpAddress();
            if (!String.IsNullOrEmpty(currentIpAddress))
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var customer = workContext.CurrentAccountUser;
                if (customer == null) return;


                if (!currentIpAddress.Equals(customer.LastIpAddress, StringComparison.InvariantCultureIgnoreCase))
                {
                    var customerService = EngineContext.Current.Resolve<IAccountUserService>();
                    customer.LastIpAddress = currentIpAddress;
                    customerService.UpdateAccountUser(customer);
                }
            }
        }
    }
}