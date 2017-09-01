using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyRentModel: BaseQMEntityModel
    {
        public string Name { get; set; }
        public string RentTime { get; set; }
        public float RentArea { get; set; }
        public int RentMonth { get; set; }
        public float RentPrice { get; set; }
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
        public DateTime BackTime { get; set; }

        public string PriceString { get; set; }
        public virtual ICollection<PropertyRentPictureModel> RentPictures { get; set; }

        public virtual ICollection<PropertyRentFileModel> RentFiles { get; set; }
    }
}