using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
   public class MonthTotal:BaseEntity
    {
        /// <summary>
        /// 资产ID
        /// </summary>
        public int Property_ID { get; set; }
        /// <summary>
        /// 资产名称
        /// </summary>
        public string Property_Name { get; set; }

        /// <summary>
        /// 自用面积
        /// </summary>
        public double CurrentUse_Self { get; set; }

        /// <summary>
        /// 出租面积
        /// </summary>
        public double CurrentUse_Rent { get; set; }

        /// <summary>
        /// 出借面积
        /// </summary>
        public double CurrentUse_Lend { get; set; }

        /// <summary>
        /// 闲置面积
        /// </summary>
        public double CurrentUse_Idle { get; set; }

        /// <summary>
        /// 账面价格，单位万元
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime Month { get; set; }
        /// <summary>
        /// 当月收入
        /// </summary>
        public double Income { get; set; }


    }
}
