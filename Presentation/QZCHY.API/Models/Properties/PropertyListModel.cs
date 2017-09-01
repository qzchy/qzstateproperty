using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyListModel: BaseQMEntityModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Region { get; set; }

        public string GetedDate { get; set; }

        public int GovernmentId { get; set; }

        public string GovernmentName { get; set; }

        public bool CanEditDelete { get; set; }

        public bool CanChange { get; set; }
    }
}