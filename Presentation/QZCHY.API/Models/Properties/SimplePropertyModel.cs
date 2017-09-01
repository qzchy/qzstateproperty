using QZCHY.Web.Framework.Mvc;

namespace QZCHY.API.Models.Properties
{
    public class SimplePropertyModel: BaseQMEntityModel
    {
        public string Name { get; set; }

        public string GovernmentName { get; set; }
    }
}