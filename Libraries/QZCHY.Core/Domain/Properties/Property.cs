using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace QZCHY.Core.Domain.Properties
{
    public class Property : BaseEntity
    {
        public Property()
        {
        }

        private ICollection<PropertyAllot> _propertyAllots;
        private ICollection<PropertyLend> _propertyLends;
        private ICollection<PropertyRent> _propertyRents;
        private ICollection<PropertyEdit> _propertyEdits;
        private ICollection<PropertyPicture> _propertyPictures;
        private ICollection<PropertyFile> _propertyFiles;

        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资产类别
        /// </summary>
        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// 资产所处区域
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public double ConstructArea { get; set; }

        /// <summary>
        /// 土地面积
        /// </summary>
        public double LandArea { get; set; }

        /// <summary>
        /// 产权证号
        /// </summary>
        public string PropertyID { get; set; }

        /// <summary>
        /// 拥有房产证
        /// </summary>
        public bool HasConstructID { get; set; }

        /// <summary>
        /// 拥有土地证
        /// </summary>
        public bool HasLandID { get; set; }

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
        public double Price { get; set; }

        /// <summary>
        /// 取得时间
        /// </summary>
        public DateTime? GetedDate { get; set; }

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
        public double CurrentUse_Self { get; set; }

        /// <summary>
        /// 出租面积
        /// </summary>
        public double CurrentUse_Rent { get; set; }

        /// <summary>
        /// 出借面积
        /// </summary>
        public double CurrentUse_Lend { get; set; }

        /// <summary>
        /// 闲置面积
        /// </summary>
        public double CurrentUse_Idle { get; set; }

        /// <summary>
        /// 下步使用或处置建议
        /// </summary>
        public NextStepType NextStepUsage { get; set; }

        /// <summary>
        /// 坐落位置
        /// </summary>
        public DbGeography Location { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        /// <summary>
        /// 范围，房产不需要
        /// </summary>
        public DbGeography Extent { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 错误
        /// </summary>
        public string Error { get; set; }

        public string WKT { get; set; }

        /// <summary>
        /// 资产所有方
        /// </summary>
        public virtual GovernmentUnit Government { get; set; }

        /// <summary>
        /// 不动产证
        /// </summary>
        public string EstateId { get; set; }

        /// <summary>
        /// 房产证
        /// </summary>
        public string ConstructId { get; set; }

        /// <summary>
        /// 土地证号
        /// </summary>
        public string LandId { get; set;}

        public virtual PropertyNewCreate PropertyNewCreate { get; set; }

        public virtual ICollection<PropertyAllot> Allots
        {
            get { return _propertyAllots ?? (_propertyAllots = new List<PropertyAllot>()); }
            protected set { _propertyAllots = value; }
        }
        public virtual ICollection<PropertyLend> Lends
        {
            get { return _propertyLends ?? (_propertyLends = new List<PropertyLend>()); }
            protected set { _propertyLends = value; }
        }
        public virtual ICollection<PropertyRent> Rents
        {
            get { return _propertyRents ?? (_propertyRents = new List<PropertyRent>()); }
            protected set { _propertyRents = value; }
        }

        public virtual ICollection<PropertyEdit> Edits
        {
            get { return _propertyEdits ?? (_propertyEdits = new List<PropertyEdit>()); }
            protected set { _propertyEdits = value; }
        }

        public virtual PropertyOff PropertyOff { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// 是否核销
        /// </summary>
        public bool Off { get; set; }

        /// <summary>
        /// 是否为Excel导入
        /// </summary>
        public bool FromExcelImport { get; set; }
        
        public virtual ICollection<PropertyPicture> Pictures
        {
            get { return _propertyPictures ?? (_propertyPictures = new List<PropertyPicture>()); }
            protected set { _propertyPictures = value; }
        }

        public virtual ICollection<PropertyFile> Files
        {
            get { return _propertyFiles ?? (_propertyFiles = new List<PropertyFile>()); }
            protected set { _propertyFiles = value; }
        }

    }
}
