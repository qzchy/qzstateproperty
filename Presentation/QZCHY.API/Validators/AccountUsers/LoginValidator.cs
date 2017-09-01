using QZCHY.Web.Framework.Validators;
using FluentValidation;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Api.Models.AccountUser;

namespace QZCHY.Api.Validators.AccountUsers
{
    public class LoginValidator:BaseQMValidator<LoginModel>
    {
        public LoginValidator(AccountUserSettings customerSettings)
        {
            if(!customerSettings.UsernamesEnabled)
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("登录账户不能为空");
            }
        }
    }
}