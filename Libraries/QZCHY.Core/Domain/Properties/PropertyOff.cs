using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
  public  class PropertyOff:BaseEntity
    {
        private ICollection<PropertyOffPicture> _propertyOffPictures;
        private ICollection<PropertyOffFile> _propertyOffFiles;

        public PropertyOff() {

        }
        public DateTime ProcessDate { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// 核销日期
        /// </summary>
        public DateTime OffTime { get; set; }
        /// <summary>
        /// 核销原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
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
        /// <summary>
        /// 核销方式
        /// </summary>
        public OffType OffType { get; set; }

        /// <summary>
        /// 资产处置状态
        /// </summary>
        public PropertyApproveState State { get; set; }
        public virtual Property Property { get; set; }
        public int Property_Id { get; set; }

        public int SuggestGovernmentId { get; set; }

        public virtual ICollection<PropertyOffPicture> OffPictures
        {
            get { return _propertyOffPictures ?? (_propertyOffPictures = new List<PropertyOffPicture>()); }
            protected set { _propertyOffPictures = value; }
        }

        public virtual ICollection<PropertyOffFile> OffFiles
        {
            get { return _propertyOffFiles ?? (_propertyOffFiles = new List<PropertyOffFile>()); }
            protected set { _propertyOffFiles = value; }
        }
    }
}
