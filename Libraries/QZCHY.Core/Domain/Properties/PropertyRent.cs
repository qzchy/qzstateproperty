using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
   public class PropertyRent:BaseEntity
    {
        private ICollection<PropertyRentPicture> _propertyRentPictures;
        private ICollection<PropertyRentFile> _propertyRentFiles;

        public PropertyRent() {

        }

        public DateTime ProcessDate { get; set; }
        /// <summary>
        /// 出租方
        /// </summary>
        public string Name { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 出租日期
        /// </summary>
        public DateTime RentTime { get; set; }
        /// <summary>
        /// 归还日期
        /// </summary>
        public DateTime BackTime { get; set; }
        /// <summary>
        /// 租金字符串
        /// </summary>
        public string PriceString { get; set; }
        /// <summary>
        /// 出租面积
        /// </summary>
        public float RentArea { get; set; }
        /// <summary>
        /// 出租时间，单位月
        /// </summary>
        public int RentMonth { get; set; }
        /// <summary>
        /// 出租金额，单位万元
        /// </summary>
        public float RentPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Ext { get; set; }
        /// <summary>
        /// 部门意见
        /// </summary>
        public string DSuggestion { get; set; }
        /// <summary>
        /// 主管部门审批时间
        /// </summary>
        public DateTime? DApproveDate { get; set; }

        /// <summary>
        /// 管理员意见
        /// </summary>
        public string ASuggestion { get; set; }

        /// <summary>
        /// 管理部门审批时间
        /// </summary>
        public DateTime? AApproveDate { get; set; }
        public PropertyApproveState State { get; set; }
        public virtual Property Property { get; set; }
        /// <summary>
        /// 申请的单位Id
        /// </summary>
        public int SuggestGovernmentId { get; set; }

        public virtual ICollection<PropertyRentPicture> RentPictures
        {
            get { return _propertyRentPictures ?? (_propertyRentPictures = new List<PropertyRentPicture>()); }
            protected set { _propertyRentPictures = value; }
        }

        public virtual ICollection<PropertyRentFile> RentFiles
        {
            get { return _propertyRentFiles ?? (_propertyRentFiles = new List<PropertyRentFile>()); }
            protected set { _propertyRentFiles = value; }
        }
    }
}
