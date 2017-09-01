using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
   public class PropertyLendMap : EntityTypeConfiguration<PropertyLend>
    {
        public PropertyLendMap()
        {
            this.ToTable("PropertyLend");
            this.HasKey(p => p.Id);
            //this.Property(p => p.ASuggestion).IsRequired();
            //this.Property(p => p.DSuggestion).IsRequired();
            //this.Property(p => p.Name).IsRequired().HasMaxLength(255);
            //this.Property(p => p.LendArea).IsRequired();
            //this.Property(p => p.LendTime).IsRequired();
        }
    }
}
