using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace QZCHY.Services.Property
{
    /// <summary>
    /// 资产高级搜索条件
    /// </summary>
    public class PropertyAdvanceConditionRequest
    {
       public PropertyAdvanceConditionRequest()
        {
            this.GovernmentTypes = new List<int>();
            this.PropertyType = new List<int>();
            this.Region = new List<int>();
            this.NextStep = new List<int>();
            this.ConstructArea = new List<List<double>>();
            this.LandArea = new List<List<double>>();
            this.Price = new List<List<double>> ();
            this.LifeTime = new List<double>();
            this.GetedDate = new List<double>();
        }

        public bool SerachParentGovernement { get; set; }

        public int GovernmentId { get; set; }

        public List<int> GovernmentTypes { get; set; }

        public List<int> PropertyType { get; set; }

        public List<int> Region { get; set; }

        #region 证书现状
        public bool Certificate_Both { get; set; }

        public bool Certificate_Land { get; set; }

        public bool Certificate_Construct { get; set; }

        public bool Certificate_None { get; set; }
        #endregion

        #region 当前使用现状
        public bool Current_Self { get; set; }

        public bool Current_Rent { get; set; }

        public bool Current_Lend { get; set; }

        public bool Current_Idle { get; set; }
        #endregion

        public List<int> NextStep { get; set; }

        public List<List<double>> ConstructArea { get; set; }

        public List<List<double>> LandArea { get; set; }

        public List<List<double>> Price { get; set; }

        public List<double> LifeTime { get; set; }

        public List<double> GetedDate { get; set; }

        public DbGeography Extent { get; set; }

    }
}
