using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyLendModel: BaseQMEntityModel
    {
        public string ProcessDate { get; set; }
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public string LendTime { get; set; }
        public float LendArea { get; set; }
        public string BackTime { get; set; }
        public string Remark { get; set; }
        public string DSuggestion { get; set; }

        public string DApproveDate { get; set; }

        public string ASuggestion { get; set; }

        public string AApproveDate { get; set; }

        public string State { get; set; }

        public string Title { get; set; }


        public int Property_Id { get; set; }

        public string Ids { get; set; }
        public bool Submit { get; set; }

        public virtual ICollection<PropertyLendPictureModel> LendPictures { get; set; }

        public virtual ICollection<PropertyLendFileModel> LendFiles { get; set; }
      
    }
}