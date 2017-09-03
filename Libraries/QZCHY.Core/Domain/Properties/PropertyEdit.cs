using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
  public  class PropertyEdit: BaseEntity
    {
        public PropertyEdit() {

        }

        public DateTime ProcessDate { get; set; }

        public string Title { get; set; }

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

        /// <summary>
        /// 申请的单位Id
        /// </summary>
        public int SuggestGovernmentId { get; set; }

        public virtual Property Property { get; set; }

        /// <summary>
        /// 变更审批通过前存储的新资产信息
        /// </summary>
        public int CopyProperty_Id { get; set; }

        /// <summary>
        /// 变更审批通过后存储的原资产信息
        /// </summary>
        public int OriginCopyProperty_Id { get; set; }
    }
}
