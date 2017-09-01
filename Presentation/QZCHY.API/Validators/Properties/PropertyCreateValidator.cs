using FluentValidation;
using QZCHY.API.Models.Properties;
using QZCHY.Web.Framework.Validators;

namespace QZCHY.API.Validators.Properties
{
    public class PropertyCreateValidator: BaseQMValidator<PropertyCreateModel>
    {
        public PropertyCreateValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("资产名称不能为空");
            RuleFor(s => s.PropertyTypeId).GreaterThan(-1).WithMessage("资产类别不能为空");
            RuleFor(s => s.UsedPeople).NotEmpty().WithMessage("使用方不能为空");
            RuleFor(s => s.Address).NotEmpty().WithMessage("坐落地址不能为空");
            //RuleFor(s => s.ConstructArea).GreaterThan(0).WithMessage("建筑面积必须大于0");
            //RuleFor(s => s.LandArea).GreaterThan(0).WithMessage("土地面积必须大于0");
            RuleFor(s => s.Price).GreaterThan(0).WithMessage("账面价格必须大于0");
            RuleFor(s => s.LifeTime).GreaterThan(0).WithMessage("使用年限必须大于0");
            RuleFor(s => s.LandNature).NotEmpty().WithMessage("土地性质不能为空");
            RuleFor(s => s.UsedPeople).NotEmpty().WithMessage("使用方名称不能为空");
            //RuleFor(s => s.CurrentUse_Self).GreaterThan(0).WithMessage("自用面积必须大于0");
            //RuleFor(s => s.CurrentUse_Rent).GreaterThan(0).WithMessage("出租面积必须大于0");
            //RuleFor(s => s.CurrentUse_Lend).GreaterThan(0).WithMessage("出借面积必须大于0");
            //RuleFor(s => s.CurrentUse_Idle).GreaterThan(0).WithMessage("闲置面积必须大于0");                    
        }

    }
}