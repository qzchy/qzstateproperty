using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyRentApproveListModel : PropertyApproveListModel
    {
        /// <summary>
        /// 出租方
        /// </summary>
        public string Name { get; set; }        
        /// <summary>
        /// 出租面积
        /// </summary>
        public float RentArea { get; set; }
        /// <summary>
        /// 出租时间，单位月
        /// </summary>
        public string RentTime { get; set; }

        public string BackTime { get; set; }

        /// <summary>
        /// 出租金额字符串
        /// </summary>
        public string PriceString { get; set; }
    }
}