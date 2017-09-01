using QZCHY.Web.Framework.Mvc;

namespace QZCHY.API.Models.Properties
{
    public class GeoPropertyModel: BaseQMEntityModel
    {
        public string Name { get; set; }

        public string GovernmentName { get; set; }

        public string Address { get; set; }

        public string Region { get; set; }

        public string PropertyType { get; set; }

        public float ConstructArea { get; set; }

        public float LandArea { get; set; }

        public float Price { get; set; }

        public int LifeTime { get; set; }

        public string Location { get; set; }

        public string Extent { get; set; }

        public float C_Self { get; set; }

        public float C_Rent { get; set; }

        public float C_Lend { get; set; }

        public float C_Idle { get; set; }

        public int Next { get; set; }
    }
}