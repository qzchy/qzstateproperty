using AutoMapper;
using QZCHY.API.Models.AccountUsers;
using QZCHY.API.Models.Properties;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Domain.Media;
using QZCHY.Core.Domain.Properties;
using QZCHY.Web.Api.Extensions;

namespace QZCHY.Web.Api.Extensions
{
    /// <summary>
    /// 实体到模型映射
    /// </summary>
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        #region 资产
        public static PropertyModel ToModel(this Property entity)
        {
            return entity.MapTo<Property, PropertyModel>();
        }

        public static Property ToEntity(this PropertyModel model)
        {
            return model.MapTo<PropertyModel, Property>();
        }

        public static Property ToEntity(this PropertyModel model, Property destination)
        {
            return model.MapTo(destination);
        }
        public static GeoPropertyModel ToGeoModel(this Property entity)
        {
            return entity.MapTo<Property, GeoPropertyModel>();
        }

        public static PropertyListModel ToListModel(this Property entity)
        {
            return entity.MapTo<Property, PropertyListModel>();
        }
        #endregion

        #region  每月表格
        public static MonthTotalModel ToModel(this MonthTotal entity)
        {
            return entity.MapTo<MonthTotal, MonthTotalModel>();
        }

        public static MonthTotal ToEntity(this MonthTotalModel model)
        {
            return model.MapTo<MonthTotalModel, MonthTotal>();
        }

        public static MonthTotal ToEntity(this MonthTotalModel model, MonthTotal destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region 资产复制
        public static PropertyModel ToModel(this CopyProperty entity)
        {
            return entity.MapTo<CopyProperty, PropertyModel>();
        }

        public static PropertyCreateModel ToCreateModel(this CopyProperty entity)
        {
            return entity.MapTo<CopyProperty, PropertyCreateModel>();
        }

        public static CopyProperty ToCopyEntity(this PropertyCreateModel model)
        {
            return model.MapTo<PropertyCreateModel, CopyProperty>();
        }

        public static CopyProperty ToEntity(this PropertyCreateModel model, CopyProperty destination)
        {
            return model.MapTo(destination);
        }
   

        //基本信息
        public static PropertyBasicInfo ToBasicModel(this Property entity)
        {
            return entity.MapTo<Property, PropertyBasicInfo>();
        }
        public static PropertyBasicInfo ToCopyBasicModel(this CopyProperty entity)
        {
            return entity.MapTo<CopyProperty, PropertyBasicInfo>();
        }

        #endregion

        #region 创建资产
        public static PropertyCreateModel ToCreateModel(this Property entity)
        {
            return entity.MapTo<Property, PropertyCreateModel>();
        }

        public static Property ToEntity(this PropertyCreateModel createmodel)
        {
            return createmodel.MapTo<PropertyCreateModel, Property>();
        }

        public static Property ToEntity(this PropertyCreateModel createmodel, Property destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion

        #region 资产新增
        public static PropertyNewCreateModel ToModel(this PropertyNewCreate entity)
        {
            return entity.MapTo<PropertyNewCreate, PropertyNewCreateModel>();
        }
        public static PropertyNewCreateApproveListModel ToListModel(this PropertyNewCreate entity)
        {
            return entity.MapTo<PropertyNewCreate, PropertyNewCreateApproveListModel>();
        }

        public static CopyProperty ToEntity(this PropertyNewCreateModel createmodel)
        {
            return createmodel.MapTo<PropertyNewCreateModel, CopyProperty>();
        }

        public static CopyProperty ToEntity(this PropertyNewCreateModel createmodel, CopyProperty destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion

        #region 资产变更
        public static PropertyEditModel ToModel(this PropertyEdit entity)
        {
            return entity.MapTo<PropertyEdit, PropertyEditModel>();
        }
        public static PropertyEditApproveListModel ToListModel(this PropertyEdit entity)
        {
            return entity.MapTo<PropertyEdit, PropertyEditApproveListModel>();
        }
        public static PropertyEdit ToEntity(this PropertyEditModel createmodel)
        {
            return createmodel.MapTo<PropertyEditModel, PropertyEdit>();
        }

        public static PropertyEdit ToEntity(this PropertyEditModel createmodel, PropertyEdit destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion

        #region 资产出借
        public static PropertyLendModel ToModel(this PropertyLend entity)
        {
            return entity.MapTo<PropertyLend, PropertyLendModel>();
        }
        public static PropertyLendApproveListModel ToListModel(this PropertyLend entity)
        {
            return entity.MapTo<PropertyLend, PropertyLendApproveListModel>();
        }
        public static PropertyLend ToEntity(this PropertyLendModel createmodel)
        {
            return createmodel.MapTo<PropertyLendModel, PropertyLend>();
        }

        public static PropertyLend ToEntity(this PropertyLendModel createmodel, PropertyLend destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion

        #region 资产出租
        public static PropertyRentModel ToModel(this PropertyRent entity)
        {
            return entity.MapTo<PropertyRent, PropertyRentModel>();
        }
        public static PropertyRentApproveListModel ToListModel(this PropertyRent entity)
        {
            return entity.MapTo<PropertyRent, PropertyRentApproveListModel>();
        }
        public static PropertyRent ToEntity(this PropertyRentModel createmodel)
        {
            return createmodel.MapTo<PropertyRentModel, PropertyRent>();
        }

        public static PropertyRent ToEntity(this PropertyRentModel createmodel, PropertyRent destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion

        #region 资产划拨
        public static PropertyAllotModel ToModel(this PropertyAllot entity)
        {
            return entity.MapTo<PropertyAllot, PropertyAllotModel>();
        }
        public static PropertyAllotApproveListModel ToListModel(this PropertyAllot entity)
        {
            return entity.MapTo<PropertyAllot, PropertyAllotApproveListModel>();
        }
        public static PropertyAllot ToEntity(this PropertyAllotModel createmodel)
        {
            return createmodel.MapTo<PropertyAllotModel, PropertyAllot>();
        }

        public static PropertyAllot ToEntity(this PropertyAllotModel createmodel, PropertyAllot destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion


        #region 资产核销
        public static PropertyOffModel ToModel(this PropertyOff entity)
        {
            return entity.MapTo<PropertyOff, PropertyOffModel>();
        }
        public static PropertyOffApproveListModel ToListModel(this PropertyOff entity)
        {
            return entity.MapTo<PropertyOff, PropertyOffApproveListModel>();
        }
        public static PropertyOff ToEntity(this PropertyOffModel createmodel)
        {
            return createmodel.MapTo<PropertyOffModel, PropertyOff>();
        }

        public static PropertyOff ToEntity(this PropertyOffModel createmodel, PropertyOff destination)
        {
            return createmodel.MapTo(destination);
        }
        #endregion

        public static PropertyPictureModel ToModel(this PropertyPicture entity)
        {
            return entity.MapTo<PropertyPicture, PropertyPictureModel>();
        }

        public static PropertyPicture ToEntity(this PropertyPictureModel model)
        {
            return model.MapTo<PropertyPictureModel, PropertyPicture>();
        }

        public static PropertyPicture ToEntity(this PropertyPictureModel model, PropertyPicture destination)
        {
            return model.MapTo(destination);
        }

        public static PropertyFileModel ToModel(this PropertyFile entity)
        {
            return entity.MapTo<PropertyFile, PropertyFileModel>();
        }

        public static PropertyFile ToEntity(this PropertyFileModel model)
        {
            return model.MapTo<PropertyFileModel, PropertyFile>();
        }

        public static PropertyFile ToEntity(this PropertyFileModel model, PropertyFile destination)
        {
            return model.MapTo(destination);
        }

        #region propertyallot
        public static PropertyAllotPictureModel ToModel(this PropertyAllotPicture entity)
        {
            return entity.MapTo<PropertyAllotPicture, PropertyAllotPictureModel>();
        }

        public static PropertyAllotPicture ToEntity(this PropertyAllotPictureModel model)
        {
            return model.MapTo<PropertyAllotPictureModel, PropertyAllotPicture>();
        }

        public static PropertyAllotPicture ToEntity(this PropertyAllotPictureModel model, PropertyAllotPicture destination)
        {
            return model.MapTo(destination);
        }

        public static PropertyAllotFileModel ToModel(this PropertyAllotFile entity)
        {
            return entity.MapTo<PropertyAllotFile, PropertyAllotFileModel>();
        }

        public static PropertyAllotFile ToEntity(this PropertyAllotFileModel model)
        {
            return model.MapTo<PropertyAllotFileModel, PropertyAllotFile>();
        }

        public static PropertyAllotFile ToEntity(this PropertyAllotFileModel model, PropertyAllotFile destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region propertylend
        public static PropertyLendPictureModel ToModel(this PropertyLendPicture entity)
        {
            return entity.MapTo<PropertyLendPicture, PropertyLendPictureModel>();
        }

        public static PropertyLendPicture ToEntity(this PropertyLendPictureModel model)
        {
            return model.MapTo<PropertyLendPictureModel, PropertyLendPicture>();
        }

        public static PropertyLendPicture ToEntity(this PropertyLendPictureModel model, PropertyLendPicture destination)
        {
            return model.MapTo(destination);
        }

        public static PropertyLendFileModel ToModel(this PropertyLendFile entity)
        {
            return entity.MapTo<PropertyLendFile, PropertyLendFileModel>();
        }

        public static PropertyLendFile ToEntity(this PropertyLendFileModel model)
        {
            return model.MapTo<PropertyLendFileModel, PropertyLendFile>();
        }

        public static PropertyLendFile ToEntity(this PropertyLendFileModel model, PropertyLendFile destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region propertyrent
        public static PropertyRentPictureModel ToModel(this PropertyRentPicture entity)
        {
            return entity.MapTo<PropertyRentPicture, PropertyRentPictureModel>();
        }

        public static PropertyRentPicture ToEntity(this PropertyRentPictureModel model)
        {
            return model.MapTo<PropertyRentPictureModel, PropertyRentPicture>();
        }

        public static PropertyRentPicture ToEntity(this PropertyRentPictureModel model, PropertyRentPicture destination)
        {
            return model.MapTo(destination);
        }

        public static PropertyRentFileModel ToModel(this PropertyRentFile entity)
        {
            return entity.MapTo<PropertyRentFile, PropertyRentFileModel>();
        }

        public static PropertyRentFile ToEntity(this PropertyRentFileModel model)
        {
            return model.MapTo<PropertyRentFileModel, PropertyRentFile>();
        }

        public static PropertyRentFile ToEntity(this PropertyRentFileModel model, PropertyRentFile destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region propertyoff
        public static PropertyOffPictureModel ToModel(this PropertyOffPicture entity)
        {
            return entity.MapTo<PropertyOffPicture, PropertyOffPictureModel>();
        }

        public static PropertyOffPicture ToEntity(this PropertyOffPictureModel model)
        {
            return model.MapTo<PropertyOffPictureModel, PropertyOffPicture>();
        }

        public static PropertyOffPicture ToEntity(this PropertyOffPictureModel model, PropertyOffPicture destination)
        {
            return model.MapTo(destination);
        }

        public static PropertyOffFileModel ToModel(this PropertyOffFile entity)
        {
            return entity.MapTo<PropertyOffFile, PropertyOffFileModel>();
        }

        public static PropertyOffFile ToEntity(this PropertyOffFileModel model)
        {
            return model.MapTo<PropertyOffFileModel, PropertyOffFile>();
        }

        public static PropertyOffFile ToEntity(this PropertyOffFileModel model, PropertyOffFile destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region 单位
        public static GovernmentUnitModel ToModel(this GovernmentUnit entity)
        {
            return entity.MapTo<GovernmentUnit, GovernmentUnitModel>();
        }

        public static GeoGorvernmentModel ToGeoModel(this GovernmentUnit entity)
        {
            return entity.MapTo<GovernmentUnit, GeoGorvernmentModel>();
        }

        public static GovernmentUnit ToEntity(this GovernmentUnitModel model)
        {
            return model.MapTo<GovernmentUnitModel, GovernmentUnit>();
        }

        public static GovernmentUnit ToEntity(this GovernmentUnitModel model, GovernmentUnit destination)
        {
            return model.MapTo(destination);
        } 
        #endregion


        #region 用户

        public static AccountUserModel ToModel(this AccountUser entity)
        {
            return entity.MapTo<AccountUser, AccountUserModel>();
        }

        public static AccountUser ToEntity(this AccountUserModel model)
        {
            return model.MapTo<AccountUserModel, AccountUser>();
        }

        public static AccountUser ToEntity(this AccountUserModel model, AccountUser destination)
        {
            return model.MapTo(destination);
        }


        public static AccountUserRoleModel ToModel(this AccountUserRole entity)
        {
            return entity.MapTo<AccountUserRole, AccountUserRoleModel>();
        }

        public static AccountUserRole ToEntity(this AccountUserRoleModel model)
        {
            return model.MapTo<AccountUserRoleModel, AccountUserRole>();
        }

        public static AccountUserRole ToEntity(this AccountUserRoleModel model, AccountUserRole destination)
        {
            return model.MapTo(destination);
        }
        #endregion
    }
}