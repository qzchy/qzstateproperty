using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyAllotModel: BaseQMEntityModel
    {
        public string PrePropertyOwner { get; set; }
        public string NowPropertyOwner { get; set; }
        public string AllotTime { get; set; }
        public string Remark { get; set; }
        public string ProcessDate { get; set; }

        public string Title { get; set; }

        public string DSuggestion { get; set; }

        public string DApproveDate { get; set; }

        public string ASuggestion { get; set; }

        public string AApproveDate { get; set; }

        public string State { get; set; }
        public int Property_Id { get; set; }
        public string Ids { get; set; }

        public bool Submit { get; set; }

        public virtual ICollection<PropertyAllotPictureModel> AllotPictures { get; set; }

        public virtual ICollection<PropertyAllotFileModel> AllotFiles { get; set; }

    }
}