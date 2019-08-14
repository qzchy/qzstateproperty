using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class ApproveApplyModel
    {
        public string ApproveType { get; set; }

        public bool Agree { get; set; }

        public string Suggestion { get; set; }

        public string idsString { get; set; }
    }
}