using QZCHY.Web.Framework.Mvc;
using System.Collections.Generic;

namespace QZCHY.API.Models.Properties
{
    public class GovernmentUnitModel : BaseQMEntityModel
    {
        public string Name { get; set; }

        public string GovernmentType { get; set; }

        public int GovernmentTypeId { get; set; }

        public string Address { get; set; }

        public string Person { get; set; }

        public string Tel { get; set; }

        public int DisplayOrder { get; set; }

        public int ParentGovernmentId { get; set; }

        public string ParentGovernment { get; set; }

        /// <summary>
        /// 土地来源
        /// </summary>
        public string LandOrigin { get; set; }

        public double LandArea { get; set; }

        public double ConstructArea { get; set; }

        public double OfficeArea { get; set; }

        public int Floor { get; set; }

       public bool HasLandCard { get; set; }

        public bool HasConstructCard { get; set; }

        public bool HasRentInCard { get; set; }

        public bool HasRentCard { get; set; }

        public bool HasLendCard { get; set; }

        public string Remark { get; set; }

        public string PeopleCount { get; set; }

        public string RealPeopleCount { get; set; }

        /// <summary>
        ///  出租面积
        /// </summary>
        public string RentArea { get; set; }

        /// <summary>
        /// 承租方
        /// </summary>
        public string RentPart { get; set; }

        /// <summary>
        /// 出租信息
        /// </summary>
        public string RentInfo { get; set; }
        /// <summary>
        /// 统一信用代码
        /// </summary>
        public string CreditCode { get; set; }

        /// <summary>
        /// 人员编制
        /// </summary>
        public int PersonNumber { get; set; }

        //public string ChildrenGorvernmentsName { get; set; }

        public IList<SimpleGovernmentUnitModel> ChildrenGorvernments { get; set; }
    }
}