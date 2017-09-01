using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
   public class PropertyRentMap : EntityTypeConfiguration<PropertyRent>
    {
        public PropertyRentMap()
        {
            this.ToTable("PropertyRent");
            this.HasKey(p => p.Id);
            //this.Property(p => p.ASuggestion).IsRequired();
            //this.Property(p => p.DSuggestion).IsRequired();
            //this.Property(p => p.Name).IsRequired().HasMaxLength(255);
            //this.Property(p => p.RentArea).IsRequired();
            //this.Property(p => p.RentMonth).IsRequired();
            //this.Property(p => p.RentPrice).IsRequired();
            //this.Property(p => p.RentTime).IsRequired();
            //this.Property(p => p.Ext).IsRequired();
        }

    }
}
