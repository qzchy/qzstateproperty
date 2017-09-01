using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyEditMap : EntityTypeConfiguration<PropertyEdit>
    {

        public PropertyEditMap()
        {
            this.ToTable("PropertyEdit");
            this.HasKey(p => p.Id);
            this.HasRequired(nc => nc.Property).WithMany(p=>p.Edits);

            //this.Property(p=>p.ASuggestion).IsRequired();
            //this.Property(p => p.DSuggestion).IsRequired();
            //this.HasRequired(p => p.Property).WithOptional(p => p.PropertyNewCreate);
        }
    }
}
