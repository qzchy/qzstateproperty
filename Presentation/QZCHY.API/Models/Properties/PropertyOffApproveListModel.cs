using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyOffApproveListModel : PropertyApproveListModel
    {
        /// <summary>
        /// 核销方式
        /// </summary>
        public string OffType { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public float Price { get; set; }
    }
}