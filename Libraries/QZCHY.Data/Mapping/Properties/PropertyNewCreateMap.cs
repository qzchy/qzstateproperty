using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyNewCreateMap : EntityTypeConfiguration<PropertyNewCreate>
    {

        public PropertyNewCreateMap()
        {
            this.ToTable("PropertyNewCreate");
            this.HasKey(p => p.Id);
            this.HasRequired(nc => nc.Property).WithMany().HasForeignKey(b => b.Property_Id);

            //this.Property(p=>p.ASuggestion).IsRequired();
            //this.Property(p => p.DSuggestion).IsRequired();
            //this.HasRequired(p => p.Property).WithOptional(p => p.PropertyNewCreate);
        }
    }
}
