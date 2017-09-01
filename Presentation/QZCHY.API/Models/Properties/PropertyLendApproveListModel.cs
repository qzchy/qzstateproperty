using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyLendApproveListModel : PropertyApproveListModel
    {
        /// <summary>
        /// 出租方
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 出租面积
        /// </summary>
        public float LendArea { get; set; }
    }
}