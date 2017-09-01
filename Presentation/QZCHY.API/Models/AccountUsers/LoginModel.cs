using FluentValidation.Attributes;
using QZCHY.Api.Validators.AccountUsers;
using QZCHY.Web.Framework.Mvc;

namespace QZCHY.Api.Models.AccountUser
{
    public class LoginModel:BaseQMModel
    {
        /// <summary>
        /// 用户名
        /// </summary>

        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}