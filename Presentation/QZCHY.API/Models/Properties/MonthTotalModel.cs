using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class MonthTotalModel
    {

        public int Property_ID { get; set; }

        public string Property_Name { get; set; }

        public double CurrentUse_Self { get; set; }

        public double CurrentUse_Rent { get; set; }

        public double CurrentUse_Lend { get; set; }

        public double CurrentUse_Idle { get; set; }

        public double Price { get; set; }
       
        public string Month { get; set; }
 
        public double Income { get; set; }

    }
}