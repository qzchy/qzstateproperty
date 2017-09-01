using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Statistics
{
    public class OverviewStatistics
    {
        public int TotalCount { get; set; }

        public int ConstructCount { get; set; }

        public int LandCount { get; set; }

        public float TotalPrice { get; set; }

        public float ConstructPrice { get; set; }

        public float LandPrice { get; set; }

        public float ConstructArea { get; set; }

        public float ConstrcutLandArea { get; set; }

        public float LandArea { get; set; }
    }
}