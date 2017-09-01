using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyOffMap : EntityTypeConfiguration<PropertyOff>
    {
        public PropertyOffMap()
        {
            this.ToTable("PropertyOff");
            this.HasKey(p => p.Id);
            //this.Property(p => p.ASuggestion).IsRequired();

            this.HasRequired(nc => nc.Property).WithMany().HasForeignKey(b => b.Property_Id);
        }
    }
}
