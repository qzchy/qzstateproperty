using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class ApproveStatisticsModel
    {
        public int NewCreate { get; set; }

        public int Edit { get; set; }

        public int Lend { get; set; }

        public int Rent { get; set; }

        public int Allot { get; set; }

        public int Off { get; set; }
    }
}