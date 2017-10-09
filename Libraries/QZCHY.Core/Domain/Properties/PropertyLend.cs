using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
    /// <summary>
    /// 资产出借
    /// </summary>
   public class PropertyLend:BaseEntity
    {
        private ICollection<PropertyLendPicture> _propertyLendPictures;
        private ICollection<PropertyLendFile> _propertyLendFiles;

        public PropertyLend() {

        }

        public DateTime ProcessDate { get; set; }

        /// <summary>
        /// 借用方
        /// </summary>
        public string Name { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 出借时间
        /// </summary>
        public DateTime LendTime { get; set; }
        /// <summary>
        /// 出借面积
        /// </summary>
        public double LendArea { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }
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
        /// 资产处置状态
        /// </summary>
        public PropertyApproveState State { get; set; }

        public virtual Property Property { get; set; }
        /// <summary>
        /// 申请的单位Id
        /// </summary>
        public int SuggestGovernmentId { get; set; }

        public virtual ICollection<PropertyLendPicture> LendPictures
        {
            get { return _propertyLendPictures ?? (_propertyLendPictures = new List<PropertyLendPicture>()); }
            protected set { _propertyLendPictures = value; }
        }

        public virtual ICollection<PropertyLendFile> LendFiles
        {
            get { return _propertyLendFiles ?? (_propertyLendFiles = new List<PropertyLendFile>()); }
            protected set { _propertyLendFiles = value; }
        }
    }
}
