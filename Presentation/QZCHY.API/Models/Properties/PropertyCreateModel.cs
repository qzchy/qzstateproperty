using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyCreateModel : BaseQMEntityModel
    {
        public PropertyCreateModel()
        {

        }

       
        public string Name { get; set; }

        public int PropertyTypeId { get; set; }
     
        public string Address { get; set; }
     
        public string UsedPeople { get; set; }
    
        public double CurrentUse_Self { get; set; }
   
        public double CurrentUse_Rent { get; set; }
    
        public double CurrentUse_Lend { get; set; }
    
        public double CurrentUse_Idle { get; set; }
   
        public bool Estate { get; set; }
 
        public string EstateId { get; set; }
  
        public string ConstructId { get; set; }
    
        public string LandId { get; set; }
    
        public double ConstructArea { get; set; }
    
        public double LandArea { get; set; }
   
        public bool Owner_self { get; set; }

        public bool Owner_children { get; set; }

        public int GovernmentId { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PropertyNature { get; set; }

        public string LandNature { get; set; }

        public string Logo { get; set; }

        public string LogoUrl { get; set; }

        public int LogoPictureId { get; set; }

        public string Location { get; set; }

        public string Extent { get; set; }

        public int LifeTime { get; set; }

        public string GetedDate { get; set; }


        public bool Submit { get; set; }

        public virtual ICollection<PropertyPictureModel> Pictures { get; set; }

        public virtual ICollection<PropertyFileModel> Files { get; set; }
    }
}