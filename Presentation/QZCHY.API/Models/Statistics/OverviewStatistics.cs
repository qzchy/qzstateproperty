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

        public double TotalPrice { get; set; }

        public double ConstructPrice { get; set; }

        public double LandPrice { get; set; }

        public double ConstructArea { get; set; }

        public double ConstrcutLandArea { get; set; }

        public double LandArea { get; set; }
    }
}