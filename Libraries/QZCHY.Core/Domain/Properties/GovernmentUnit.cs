using QZCHY.Core.Domain.AccountUsers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Spatial;

namespace QZCHY.Core.Domain.Properties
{
    public class GovernmentUnit : BaseEntity
    {
        private ICollection<Property> _properties;
        private ICollection<AccountUser> _accountUsers;

        public GovernmentUnit()
        {

        }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位性质
        /// </summary>
        public GovernmentType GovernmentType { get; set; }

        /// <summary>
        /// 是否为主管部门
        /// </summary>
        public bool IsChargeDepartment { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Person { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Tel { get; set; }

        public int PeopleCount { get; set; }

        public int RealPeopleCount { get; set; }

        /// <summary>
        /// 空间位置
        /// </summary>
        public DbGeography Location { get; set; }

        /// <summary>
        /// 土地来源
        /// </summary>
        public string LandOrigin { get; set; }

        public double? LandArea { get; set; }

        public double? ConstructArea { get; set; }

        public double? OfficeArea { get; set; }

        public int? Floor { get; set; }

        public bool HasLandCard { get; set; }

        public bool HasConstructCard { get; set; }

        public bool HasRentInCard { get; set; }

        public bool HasRentCard { get; set; }

        public bool HasLendInCard { get; set; }

        public string Remark { get; set; }


        public int DisplayOrder { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }

        /// <summary>
        ///  出租面积
        /// </summary>
        public float? RentArea { get; set; }

        /// <summary>
        /// 承租方
        /// </summary>
        public string RentPart { get; set; }

        /// <summary>
        /// 出租信息
        /// </summary>
        public string RentInfo { get; set; }

        /// <summary>
        /// 主管部门
        /// </summary>
        public int ParentGovernmentId { get; set; }
        /// <summary>
        /// 统一信用代码
        /// </summary>
        public string CreditCode { get; set; }

        /// <summary>
        /// 人员编制
        /// </summary>
        public int PersonNumber { get; set; }

        /// <summary>
        /// 资产
        /// </summary>
        public virtual ICollection<Property> Properties
        {
            get { return _properties ?? (_properties = new List<Property>()); }
            protected set { _properties = value; }
        }

        public virtual ICollection<AccountUser> Users
        {
            get { return _accountUsers ?? (_accountUsers = new List<AccountUser>()); }
            protected set { _accountUsers = value; }
        }
        public int PropertyConut { get; set; }

        public string ParentName { get; set; }

    }
}