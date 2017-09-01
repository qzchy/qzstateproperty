using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyAllotApproveListModel : PropertyApproveListModel
    {
        /// <summary>
        /// 原产权方
        /// </summary>
        public string PrePropertyOwner { get; set; }
        /// <summary>
        /// 现产权方
        /// </summary>
        public string NowPropertyOwner { get; set; }
    }
}