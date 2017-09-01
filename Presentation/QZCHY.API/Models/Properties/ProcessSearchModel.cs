using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class ProcessSearchModel
    {
        public string GovermentName { get; set; }

      public bool isGovernment { get; set; }
        public bool isInstitution { get; set; }
        public bool isCompany { get; set; }
        public bool construct { get; set; }
        public bool land { get; set; }
        public bool constructOnLand { get; set; }
        public bool old { get; set; }
        public bool west { get; set; }
        public bool jjq { get; set; }
        public bool kc { get; set; }
        public bool qj { get; set; }
        public bool other { get; set; }
    }
}