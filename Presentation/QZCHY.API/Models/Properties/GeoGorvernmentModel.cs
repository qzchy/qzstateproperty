using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class GeoGorvernmentModel : BaseQMEntityModel
    {
        public string Name { get; set; }

        public string GovernmentType { get; set; }

        public int GovernmentTypeId { get; set; }

        public string Address { get; set; }

        public string Person { get; set; }

        public string Tel { get; set; }

        public string Location { get; set; }
    }
}