using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
    /// <summary>
    /// 资产划拨
    /// </summary>
   public class PropertyAllot:BaseEntity
    {
        private ICollection<PropertyAllotPicture> _propertyAllotPictures;
        private ICollection<PropertyAllotFile> _propertyAllotFiles;

        public PropertyAllot() {

        }
        public DateTime ProcessDate { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 原产权方
        /// </summary>
        public string PrePropertyOwner { get; set; }
        /// <summary>
        /// 现产权方
        /// </summary>
        public string NowPropertyOwner { get; set; }

        public int NowGovernmentId { get; set; }

        /// <summary>
        /// 划拨时间
        /// </summary>
        public DateTime AllotTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 部门意见
        /// </summary>
        public string DSuggestion { get; set; }
        /// <summary>
        /// 管理员意见
        /// </summary>
        public string ASuggestion { get; set; }
        /// <summary>
        /// 资产处置状态
        /// </summary>
        public PropertyApproveState State { get; set; }
        public virtual Property Property { get; set; }

        public DateTime? DApproveDate { get; set; }
        public DateTime? AApproveDate { get; set; }

        /// <summary>
        /// 申请的单位Id
        /// </summary>
        public int SuggestGovernmentId { get; set; }

        public virtual ICollection<PropertyAllotPicture> AllotPictures
        {
            get { return _propertyAllotPictures ?? (_propertyAllotPictures = new List<PropertyAllotPicture>()); }
            protected set { _propertyAllotPictures = value; }
        }

        public virtual ICollection<PropertyAllotFile> AllotFiles
        {
            get { return _propertyAllotFiles ?? (_propertyAllotFiles = new List<PropertyAllotFile>()); }
            protected set { _propertyAllotFiles = value; }
        }
    }
}
