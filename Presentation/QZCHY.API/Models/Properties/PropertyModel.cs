using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyModel: BaseQMEntityModel
    {
        public PropertyModel()
        {
        }

        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资产类别
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// 资产类别枚举
        /// </summary>
        public int PropertyTypeId { get; set; }

        /// <summary>
        /// 资产所处区域
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 资产所处区域枚举
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public float ConstructArea { get; set; }

        /// <summary>
        /// 土地面积
        /// </summary>
        public float LandArea { get; set; }

        /// <summary>
        /// 产权证号
        /// </summary>
        public string PropertyID { get; set; }

        public string EstateId { get; set; }

        public string ConstructId { get; set; }

        public string LandId { get; set; }

        /// <summary>
        /// 房产性质
        /// </summary>
        public string PropertyNature { get; set; }

        /// <summary>
        /// 土地性质
        /// </summary>
        public string LandNature { get; set; }

        /// <summary>
        /// 账面价格，单位万元
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// 取得时间
        /// </summary>
        public string GetedDate { get; set; }

        /// <summary>
        /// 使用年限
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// 使用方
        /// </summary>
        public string UsedPeople { get; set; }

        /// <summary>
        /// 自用面积
        /// </summary>
        public float CurrentUse_Self { get; set; }

        /// <summary>
        /// 出租面积
        /// </summary>
        public float CurrentUse_Rent { get; set; }

        /// <summary>
        /// 出借面积
        /// </summary>
        public float CurrentUse_Lend { get; set; }

        /// <summary>
        /// 闲置面积
        /// </summary>
        public float CurrentUse_Idle { get; set; }

        /// <summary>
        /// 下步使用或处置建议
        /// </summary>
        public string NextStepUsage { get; set; }

        /// <summary>
        /// 下步使用或处置建议枚举
        /// </summary>
        public int NextStepUsageId { get; set; }

        /// <summary>
        /// 坐落位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 范围，房产不需要
        /// </summary>
        public string Extent { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// 错误
        /// </summary>
        public string Error { get; set; }

        public string WKT { get; set; }

        /// <summary>
        /// 资产所有方
        /// </summary>
        public int GovernmentId { get; set; }

        public string GovernmentName { get; set; }

        public bool Locked { get; set; }

        public string LogoUrl { get; set; }

        public bool CanEditDelete { get; set; }

        public bool CanChange { get; set; }

        public PropertyNewCreateModel NewCreate { get; set; }
        public IList<PropertyEditModel> Edits { get; set; }

        public IList<PropertyRentModel> Rents { get; set; }
        public IList<PropertyLendModel> Lends { get; set; }
        public IList<PropertyAllotModel> Allots { get; set; }

        public PropertyOffModel PropertyOff { get; set; }


        public virtual ICollection<PropertyPictureModel> Pictures { get; set; }

        public virtual ICollection<PropertyFileModel> Files { get; set; }

        public bool Off { get; set; }


    }
}