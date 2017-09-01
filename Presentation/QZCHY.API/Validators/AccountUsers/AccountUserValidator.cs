using FluentValidation;
using QZCHY.API.Models.AccountUsers;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Property;
using QZCHY.Web.Framework.Validators;

namespace QZCHY.API.Validators.AccountUsers
{
    public class AccountUserValidator : BaseQMValidator<AccountUserModel>
    {
        private readonly IAccountUserService _accountUserService = null;
        private readonly IGovernmentService _governmentService = null;

        public AccountUserValidator(IAccountUserService accountUserService, IGovernmentService governmentService)
        {
            _accountUserService = accountUserService;
            _governmentService = governmentService;

            RuleFor(s => s.UserName).NotEmpty().WithMessage("用户名不能为空").Must(BeUniqueName).WithMessage("名称 {0} 已存在", s => s.UserName);
            
            RuleFor(s => s.GovernmentId).Must(governmentId =>
            {
                var accountUserGovernment = governmentService.GetGovernmentUnitById(governmentId);
                return accountUserGovernment != null && !accountUserGovernment.Deleted;
            }).WithMessage(string.Format("单位不存在"));
        }

        private bool BeUniqueName(AccountUserModel accountUserModel, string accountUserName)
        {
            return _accountUserService.NameUniqueCheck(accountUserName, accountUserModel.Id);
        }
    }
}