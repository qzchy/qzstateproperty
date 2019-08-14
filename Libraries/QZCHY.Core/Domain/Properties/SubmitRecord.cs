using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
   public  class SubmitRecord:BaseEntity
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public int Goverment_ID { get; set; }
        /// <summary>
        /// 当前年月
        /// </summary>
        public string RecordDate { get; set; }

    }
}
