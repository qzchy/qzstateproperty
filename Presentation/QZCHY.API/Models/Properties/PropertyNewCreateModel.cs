using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyNewCreateModel: BaseQMEntityModel
    {
        public string ProcessDate { get; set; }

        public string Title { get; set; }

        public string DSuggestion { get; set; }

        public string DApproveDate { get; set; }

        public string ASuggestion { get; set; }

        public string AApproveDate { get; set; }

        public string State { get; set; }

        public int Property_Id { get; set; }

    }
}