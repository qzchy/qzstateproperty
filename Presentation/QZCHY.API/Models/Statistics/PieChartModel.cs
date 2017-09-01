using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Statistics
{
    public class PieChartModel
    {
        public IList<PieChartItemlModel> Data { get; set; }
    }

    public class PieChartItemlModel
    {
        public string Name { get; set; }

        public float Value { get; set; }
    }
}