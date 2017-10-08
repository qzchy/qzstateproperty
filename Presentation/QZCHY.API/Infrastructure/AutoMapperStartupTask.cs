using System;
using System.Linq;
using QZCHY.Core.Infrastructure;
using AutoMapper;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.API.Models.AccountUsers;
using QZCHY.API.Models.Properties;
using QZCHY.Core.Domain.Properties;
using QZCHY.Core;
using System.Data.Entity.Spatial;

namespace QZCHY.Web.Api.Infrastructure
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public int Order
        {
            get { return 0; }
        }

        public void Execute()
        {
            //用户对象映射
            Mapper.CreateMap<AccountUserModel, AccountUser>()
                .ForMember(dest => dest.LastActivityDate, mo => mo.Ignore())
                .ForMember(dest => dest.LastLoginDate, mo => mo.Ignore())
                .ForMember(dest => dest.AccountUserRoles, mo => mo.Ignore());

            Mapper.CreateMap<AccountUser, AccountUserModel>()
               .ForMember(dest => dest.RoleList, mo => mo.Ignore())
              .ForMember(dest => dest.LastActivityDate, mo => mo.MapFrom(src => src.LastActivityDate.HasValue ? src.LastActivityDate.Value.ToString("yyyy-MM-dd HH:mm") : ""))
              .ForMember(dest => dest.LastLoginDate, mo => mo.MapFrom(src => src.LastLoginDate.HasValue ? src.LastLoginDate.Value.ToString("yyyy-MM-dd HH:mm") : ""));


            //用户角色
            Mapper.CreateMap<AccountUserRoleModel, AccountUserRole>();

            Mapper.CreateMap<AccountUserRole, AccountUserRoleModel>();

            //单位映射
            Mapper.CreateMap<GovernmentUnitModel, GovernmentUnit>()
                .ForMember(dest => dest.GovernmentType, mo => mo.MapFrom(src => (GovernmentType)src.GovernmentTypeId));
            Mapper.CreateMap<GovernmentUnit, GovernmentUnitModel>()
                .ForMember(dest => dest.GovernmentTypeId, mo => mo.MapFrom(src => Convert.ToInt32(src.GovernmentType)))
              .ForMember(dest => dest.GovernmentType, mo => mo.MapFrom(src => src.GovernmentType.ToDescription()))
                   .ForMember(dest => dest.LandArea, mo => mo.MapFrom(src => src.LandArea.HasValue ? src.LandArea.Value : 0))
                   .ForMember(dest => dest.ConstructArea, mo => mo.MapFrom(src => src.ConstructArea.HasValue ? src.ConstructArea.Value : 0))
                   .ForMember(dest => dest.OfficeArea, mo => mo.MapFrom(src => src.OfficeArea.HasValue ? src.OfficeArea.Value : 0))
                   .ForMember(dest => dest.Floor, mo => mo.MapFrom(src => src.Floor.HasValue ? src.Floor.Value : 0))
                   .ForMember(dest => dest.HasLandCard, mo => mo.MapFrom(src => src.HasLandCard ? true : false))
                   .ForMember(dest => dest.HasConstructCard, mo => mo.MapFrom(src => src.HasConstructCard ? true : false))
                   .ForMember(dest => dest.HasRentInCard, mo => mo.MapFrom(src => src.HasRentInCard ? true : false))
                   .ForMember(dest => dest.HasRentCard, mo => mo.MapFrom(src => src.HasRentCard ? true : false))
                   .ForMember(dest => dest.HasLendCard, mo => mo.MapFrom(src => src.HasLendInCard ? true : false))
                   .ForMember(dest => dest.PeopleCount, mo => mo.MapFrom(src => src.PeopleCount == 0 ? "" : src.PeopleCount.ToString()))
                   .ForMember(dest => dest.RealPeopleCount, mo => mo.MapFrom(src => src.PeopleCount == 0 ? "" : src.RealPeopleCount.ToString()))
                   .ForMember(dest => dest.RentArea, mo => mo.MapFrom(src => src.RentArea.HasValue ? src.RentArea.Value.ToString() : ""));


            Mapper.CreateMap<GovernmentUnit, GeoGorvernmentModel>()
                    .ForMember(dest => dest.GovernmentTypeId, mo => mo.MapFrom(src => Convert.ToInt32(src.GovernmentType)))
                    .ForMember(dest => dest.GovernmentType, mo => mo.MapFrom(src => src.GovernmentType.ToDescription()))
               .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location.AsText()));

            //资产映射
            Mapper.CreateMap<PropertyModel, Property>()
           .ForMember(dest => dest.GetedDate, mo => mo.Ignore())
           .ForMember(dest => dest.Location, mo => mo.Ignore())
           .ForMember(dest => dest.Extent, mo => mo.Ignore())
              .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.PropertyTypeId))
              .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyTypeId))
              .ForMember(dest => dest.NextStepUsage, mo => mo.MapFrom(src => src.NextStepUsageId));

            Mapper.CreateMap<Property, PropertyModel>()
              //.ForMember(dest => dest.GovernmentName, mo => mo.MapFrom(src => src.Region.ToDescription()))
              .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.Region.ToDescription()))
              .ForMember(dest => dest.RegionId, mo => mo.MapFrom(src => src.Region))
              .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyType.ToDescription()))
              .ForMember(dest => dest.PropertyTypeId, mo => mo.MapFrom(src => src.PropertyType))
              .ForMember(dest => dest.NextStepUsage, mo => mo.MapFrom(src => src.NextStepUsage.ToDescription()))
              .ForMember(dest => dest.NextStepUsageId, mo => mo.MapFrom(src => src.NextStepUsage))
              .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src => src.GetedDate.HasValue ? src.GetedDate.Value.ToString("yyyy-MM-dd") : "未知"))
           .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location.AsText()))
           .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent.AsText()));

            //创建资产复制映射
            Mapper.CreateMap<PropertyCreateModel, CopyProperty>()
                     .ForMember(dest => dest.Id, mo => mo.Ignore())
                     .ForMember(dest => dest.Government_Id, mo => mo.MapFrom(src => src.GovernmentId))
           .ForMember(dest => dest.GetedDate, mo => mo.Ignore())
           .ForMember(dest => dest.Region, mo => mo.Ignore())
           .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyTypeId));


            Mapper.CreateMap<CopyProperty, PropertyCreateModel>()
                     .ForMember(dest => dest.Id, mo => mo.MapFrom(src=>src.Property_Id))
                     .ForMember(dest => dest.GovernmentId, mo => mo.MapFrom(src => src.Government_Id))
           .ForMember(dest => dest.Estate, mo => mo.MapFrom(src => !string.IsNullOrEmpty(src.EstateId)))
           .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src => src.GetedDate.HasValue ? src.GetedDate.Value.ToString("MM/dd/yyyy") : DateTime.MinValue.ToString("MM/dd/yyyy")))
              .ForMember(dest => dest.PropertyTypeId, mo => mo.MapFrom(src => src.PropertyType))
           .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location))
           .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent));


            Mapper.CreateMap<Property, PropertyListModel>()
              //.ForMember(dest => dest.GovernmentName, mo => mo.MapFrom(src => src.Region.ToDescription()))
              .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.Region.ToDescription()))
              .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src => src.GetedDate.HasValue ? src.GetedDate.Value.ToString("yyyy-MM-dd") : "未知"));

            Mapper.CreateMap<Property, PropertyBasicInfo>()
              //.ForMember(dest => dest.GovernmentName, mo => mo.MapFrom(src => src.Region.ToDescription()))
              .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.Region.ToDescription()))
              .ForMember(dest => dest.RegionId, mo => mo.MapFrom(src => src.Region))
              .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyType.ToDescription()))
              .ForMember(dest => dest.PropertyTypeId, mo => mo.MapFrom(src => src.PropertyType))
              //.ForMember(dest => dest.NextStepUsage, mo => mo.MapFrom(src => src.NextStepUsage.ToDescription()))
              //.ForMember(dest => dest.NextStepUsageId, mo => mo.MapFrom(src => src.NextStepUsage))
              .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src => src.GetedDate.HasValue ? src.GetedDate.Value.ToString("yyyy-MM-dd") : "未知"))
           .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location.AsText()))
           .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent.AsText()));

            Mapper.CreateMap<CopyProperty, PropertyModel>()
                       .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.Region.ToDescription()))
                       .ForMember(dest => dest.RegionId, mo => mo.MapFrom(src => src.Region))
                         .ForMember(dest => dest.GovernmentId, mo => mo.MapFrom(src => src.Government_Id))
                       .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyType.ToDescription()))
                       .ForMember(dest => dest.PropertyTypeId, mo => mo.MapFrom(src => src.PropertyType))
                       //.ForMember(dest => dest.NextStepUsage, mo => mo.MapFrom(src => src.NextStepUsage.ToDescription()))
                       //.ForMember(dest => dest.NextStepUsageId, mo => mo.MapFrom(src => src.NextStepUsage))
                       .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src => src.GetedDate.HasValue ? src.GetedDate.Value.ToString("yyyy-MM-dd") : "未知"))
                    .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location))
                    .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent));

            Mapper.CreateMap<CopyProperty, PropertyBasicInfo>()
             //.ForMember(dest => dest.GovernmentName, mo => mo.MapFrom(src => src.Region.ToDescription()))
             .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.Region.ToDescription()))
             .ForMember(dest => dest.RegionId, mo => mo.MapFrom(src => src.Region))
             .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyType.ToDescription()))
             .ForMember(dest => dest.PropertyTypeId, mo => mo.MapFrom(src => src.PropertyType))
             //.ForMember(dest => dest.NextStepUsage, mo => mo.MapFrom(src => src.NextStepUsage.ToDescription()))
             //.ForMember(dest => dest.NextStepUsageId, mo => mo.MapFrom(src => src.NextStepUsage))
             .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src => src.GetedDate.HasValue ? src.GetedDate.Value.ToString("yyyy-MM-dd") : "未知"))
          .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location))
          .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent));


            //创建资产映射
            Mapper.CreateMap<PropertyCreateModel, Property>()
            .ForMember(dest => dest.Pictures, mo => mo.Ignore())
            .ForMember(dest => dest.Files, mo => mo.Ignore())
           .ForMember(dest => dest.GetedDate, mo => mo.Ignore())
           .ForMember(dest => dest.Location, mo => mo.Ignore())
           .ForMember(dest => dest.Extent, mo => mo.Ignore())
              .ForMember(dest => dest.Region, mo => mo.Ignore())
              .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyTypeId));


            Mapper.CreateMap<Property, PropertyCreateModel>()
           .ForMember(dest => dest.Owner_self, mo => mo.MapFrom(src=>src.Government.Id==0))
           .ForMember(dest => dest.Estate, mo => mo.MapFrom(src => !string.IsNullOrEmpty(src.EstateId)))
           .ForMember(dest => dest.GetedDate, mo => mo.MapFrom(src=>src.GetedDate.HasValue?src.GetedDate.Value.ToString("MM/dd/yyyy"):DateTime.MinValue.ToString("MM/dd/yyyy")))
              .ForMember(dest => dest.PropertyTypeId, mo => mo.MapFrom(src => src.PropertyType))           
           .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location.AsText()))
           .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent.AsText()));

            //资产新增意见表
            Mapper.CreateMap<PropertyNewCreateModel, PropertyNewCreate>()
            .ForMember(dest => dest.ProcessDate, mo => mo.Ignore())
            .ForMember(dest => dest.State, mo => mo.Ignore())
            .ForMember(dest => dest.AApproveDate, mo => mo.Ignore())
            .ForMember(dest => dest.DApproveDate, mo => mo.Ignore());

            Mapper.CreateMap<PropertyNewCreate, PropertyNewCreateModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.DApproveDate, mo => mo.MapFrom(src => src.DApproveDate.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.AApproveDate, mo => mo.MapFrom(src => src.AApproveDate.HasValue ? src.AApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""));

            Mapper.CreateMap<PropertyNewCreate, PropertyNewCreateApproveListModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()));

            //资产编辑
            Mapper.CreateMap<PropertyEditModel, PropertyEdit>()
            .ForMember(dest => dest.ProcessDate, mo => mo.Ignore())
            .ForMember(dest => dest.State, mo => mo.Ignore())
            .ForMember(dest => dest.AApproveDate, mo => mo.Ignore())
            .ForMember(dest => dest.DApproveDate, mo => mo.Ignore());

            Mapper.CreateMap<PropertyEdit, PropertyEditModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.DApproveDate, mo => mo.MapFrom(src => src.DApproveDate.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.AApproveDate, mo => mo.MapFrom(src => src.AApproveDate.HasValue ? src.AApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""));

            Mapper.CreateMap<PropertyEdit, PropertyEditApproveListModel>()
          .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
          .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()));

            //资产划拨
            Mapper.CreateMap<PropertyAllotModel, PropertyAllot>()
            .ForMember(dest => dest.AllotPictures, mo => mo.Ignore())
            .ForMember(dest => dest.AllotFiles, mo => mo.Ignore())
              .ForMember(dest => dest.State, mo => mo.Ignore())
                .ForMember(dest => dest.Title, mo => mo.Ignore())
                .ForMember(dest => dest.ProcessDate, mo => mo.Ignore())
                 .ForMember(dest => dest.NowPropertyOwner, mo => mo.MapFrom(src=>src.NowPropertyOwner))
                .ForMember(dest => dest.DSuggestion, mo => mo.Ignore())
                .ForMember(dest => dest.ASuggestion, mo => mo.Ignore())
                   .ForMember(dest => dest.DApproveDate, mo => mo.Ignore())
                    .ForMember(dest => dest.AllotTime, mo => mo.MapFrom(src => Convert.ToDateTime(src.AllotTime)))
                .ForMember(dest => dest.AApproveDate, mo => mo.Ignore());

            Mapper.CreateMap<PropertyAllot, PropertyAllotModel>()
     .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.DApproveDate, mo => mo.MapFrom(src => src.DApproveDate.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.AApproveDate, mo => mo.MapFrom(src => src.AApproveDate.HasValue ? src.AApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
                   .ForMember(dest => dest.AllotTime, mo => mo.MapFrom(src => src.AllotTime.ToString("yyyy-MM-dd HH:mm:ss")));

            Mapper.CreateMap<PropertyAllot, PropertyAllotApproveListModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()));

            //资产出借
            Mapper.CreateMap<PropertyLendModel, PropertyLend>()
            .ForMember(dest => dest.LendPictures, mo => mo.Ignore())
            .ForMember(dest => dest.LendFiles, mo => mo.Ignore())
            .ForMember(dest => dest.State, mo => mo.Ignore())
                .ForMember(dest => dest.Title, mo => mo.Ignore())
                .ForMember(dest => dest.ProcessDate, mo => mo.Ignore())
                .ForMember(dest => dest.DSuggestion, mo => mo.Ignore())
                .ForMember(dest => dest.ASuggestion, mo => mo.Ignore())
                .ForMember(dest => dest.LendTime, mo => mo.MapFrom(src => Convert.ToDateTime(src.LendTime)))
                   .ForMember(dest => dest.DApproveDate, mo => mo.Ignore())
                .ForMember(dest => dest.AApproveDate, mo => mo.Ignore());

            Mapper.CreateMap<PropertyLend, PropertyLendModel>()
                   .ForMember(dest => dest.LendTime, mo => mo.MapFrom(src => src.LendTime.ToString("yyyy-MM-dd")))
                   .ForMember(dest => dest.BackTime, mo => mo.MapFrom(src => src.BackTime.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd"):""))
                    .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.DApproveDate, mo => mo.MapFrom(src => src.DApproveDate.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.AApproveDate, mo => mo.MapFrom(src => src.AApproveDate.HasValue ? src.AApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""));

            Mapper.CreateMap<PropertyLend, PropertyLendApproveListModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()));

            //资产出租
            Mapper.CreateMap<PropertyRentModel, PropertyRent>()
            .ForMember(dest => dest.RentPictures, mo => mo.Ignore())
            .ForMember(dest => dest.RentFiles, mo => mo.Ignore())
                .ForMember(dest => dest.State, mo => mo.Ignore())
                .ForMember(dest => dest.Title, mo => mo.Ignore())
                .ForMember(dest => dest.ProcessDate, mo => mo.Ignore())
                 .ForMember(dest => dest.RentTime, mo => mo.MapFrom(src =>Convert.ToDateTime( src.RentTime)))
                  .ForMember(dest => dest.BackTime, mo => mo.MapFrom(src => Convert.ToDateTime(src.BackTime)))
                .ForMember(dest => dest.DSuggestion, mo => mo.Ignore())
                .ForMember(dest => dest.ASuggestion, mo => mo.Ignore())
                   .ForMember(dest => dest.DApproveDate, mo => mo.Ignore())
                .ForMember(dest => dest.AApproveDate, mo => mo.Ignore());

            Mapper.CreateMap<PropertyRent, PropertyRentModel>()
                   .ForMember(dest => dest.RentTime, mo => mo.MapFrom(src => src.RentTime.ToString("yyyy-MM-dd")))
     .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.DApproveDate, mo => mo.MapFrom(src => src.DApproveDate.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.AApproveDate, mo => mo.MapFrom(src => src.AApproveDate.HasValue ? src.AApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""));

            Mapper.CreateMap<PropertyRent, PropertyRentApproveListModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()));

            //资产核销
            Mapper.CreateMap<PropertyOffModel, PropertyOff>()
            .ForMember(dest => dest.OffPictures, mo => mo.Ignore())
            .ForMember(dest => dest.OffFiles, mo => mo.Ignore())
             .ForMember(dest => dest.State, mo => mo.Ignore())
              .ForMember(dest => dest.OffType, mo => mo.Ignore())
                .ForMember(dest => dest.Title, mo => mo.Ignore())
                .ForMember(dest => dest.ProcessDate, mo => mo.Ignore())
                .ForMember(dest => dest.DSuggestion, mo => mo.Ignore())
                 .ForMember(dest => dest.OffTime, mo => mo.MapFrom(src => Convert.ToDateTime(src.OffTime)))
                .ForMember(dest => dest.ASuggestion, mo => mo.Ignore())
                   .ForMember(dest => dest.DApproveDate, mo => mo.Ignore())
                .ForMember(dest => dest.AApproveDate, mo => mo.Ignore());

            Mapper.CreateMap<PropertyOff, PropertyOffModel>()
                   .ForMember(dest => dest.OffTime, mo => mo.MapFrom(src => src.OffTime.ToString("yyyy-MM-dd")))
     .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.DApproveDate, mo => mo.MapFrom(src => src.DApproveDate.HasValue ? src.DApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.AApproveDate, mo => mo.MapFrom(src => src.AApproveDate.HasValue ? src.AApproveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
            .ForMember(dest => dest.OffType, mo => mo.MapFrom(src => src.OffType.ToDescription()));

            Mapper.CreateMap<PropertyOff, PropertyOffApproveListModel>()
            .ForMember(dest => dest.ProcessDate, mo => mo.MapFrom(src => src.ProcessDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.State, mo => mo.MapFrom(src => src.State.ToDescription()))
            .ForMember(dest => dest.OffType, mo => mo.MapFrom(src => src.OffType.ToDescription()));


            Mapper.CreateMap<Property, GeoPropertyModel>()
            .ForMember(dest => dest.Region, mo => mo.MapFrom(src => src.Region.ToDescription()))
            .ForMember(dest => dest.PropertyType, mo => mo.MapFrom(src => src.PropertyType.ToDescription()))
           .ForMember(dest => dest.Location, mo => mo.MapFrom(src => src.Location == null ? "" : src.Location.AsText()))
           .ForMember(dest => dest.Extent, mo => mo.MapFrom(src => src.Extent == null ? "" : src.Extent.AsText()))
           .ForMember(dest => dest.C_Self, mo => mo.MapFrom(src => src.CurrentUse_Self))
           .ForMember(dest => dest.C_Lend, mo => mo.MapFrom(src => src.CurrentUse_Lend))
           .ForMember(dest => dest.C_Rent, mo => mo.MapFrom(src => src.CurrentUse_Rent))
           .ForMember(dest => dest.C_Idle, mo => mo.MapFrom(src => src.CurrentUse_Idle))
           .ForMember(dest => dest.Next, mo => mo.MapFrom(src => src.NextStepUsage));

            Mapper.CreateMap<PropertyPictureModel, PropertyPicture>();
            Mapper.CreateMap<PropertyPicture, PropertyPictureModel>()
              .ForMember(dest => dest.SeoFilename, mo => mo.MapFrom(src => src.Picture.SeoFilename))
              .ForMember(dest => dest.Alt, mo => mo.MapFrom(src => src.Picture.AltAttribute))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.Picture.TitleAttribute));

            Mapper.CreateMap<PropertyFileModel, PropertyFile>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId));
            Mapper.CreateMap<PropertyFile, PropertyFileModel>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.File.Name));

            Mapper.CreateMap<PropertyAllotPictureModel, PropertyAllotPicture>();
            Mapper.CreateMap<PropertyAllotPicture, PropertyAllotPictureModel>()
              .ForMember(dest => dest.SeoFilename, mo => mo.MapFrom(src => src.Picture.SeoFilename))
              .ForMember(dest => dest.Alt, mo => mo.MapFrom(src => src.Picture.AltAttribute))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.Picture.TitleAttribute));

            Mapper.CreateMap<PropertyAllotFileModel, PropertyAllotFile>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId));
            Mapper.CreateMap<PropertyAllotFile, PropertyAllotFileModel>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.File.Name));

            Mapper.CreateMap<PropertyLendPictureModel, PropertyLendPicture>();
            Mapper.CreateMap<PropertyLendPicture, PropertyLendPictureModel>()
              .ForMember(dest => dest.SeoFilename, mo => mo.MapFrom(src => src.Picture.SeoFilename))
              .ForMember(dest => dest.Alt, mo => mo.MapFrom(src => src.Picture.AltAttribute))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.Picture.TitleAttribute));

            Mapper.CreateMap<PropertyLendFileModel, PropertyLendFile>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId));
            Mapper.CreateMap<PropertyLendFile, PropertyLendFileModel>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.File.Name));

            Mapper.CreateMap<PropertyRentPictureModel, PropertyRentPicture>();
            Mapper.CreateMap<PropertyRentPicture, PropertyRentPictureModel>()
              .ForMember(dest => dest.SeoFilename, mo => mo.MapFrom(src => src.Picture.SeoFilename))
              .ForMember(dest => dest.Alt, mo => mo.MapFrom(src => src.Picture.AltAttribute))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.Picture.TitleAttribute));

            Mapper.CreateMap<PropertyRentFileModel, PropertyRentFile>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId));
            Mapper.CreateMap<PropertyRentFile, PropertyRentFileModel>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.File.Name));

            Mapper.CreateMap<PropertyOffPictureModel, PropertyOffPicture>();
            Mapper.CreateMap<PropertyOffPicture, PropertyOffPictureModel>()
              .ForMember(dest => dest.SeoFilename, mo => mo.MapFrom(src => src.Picture.SeoFilename))
              .ForMember(dest => dest.Alt, mo => mo.MapFrom(src => src.Picture.AltAttribute))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.Picture.TitleAttribute));

            Mapper.CreateMap<PropertyOffFileModel, PropertyOffFile>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId));
            Mapper.CreateMap<PropertyOffFile, PropertyOffFileModel>()
              .ForMember(dest => dest.FileId, mo => mo.MapFrom(src => src.FileId))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.File.Name));

        }
    }
}