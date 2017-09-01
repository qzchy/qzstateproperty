using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyAllotMap : EntityTypeConfiguration<PropertyAllot>
    {
        public PropertyAllotMap()
        {
            this.ToTable("PropertyAllot");
            this.HasKey(p => p.Id);
            //this.Property(p => p.ASuggestion).IsRequired();
            //this.Property(p => p.DSuggestion).IsRequired();
            //this.Property(p => p.PrePropertyName).IsRequired().HasMaxLength(255);
            //this.Property(p => p.NowPropertyName).IsRequired();
            //this.Property(p => p.AllotTime).IsRequired();
            //this.Property(p => p.Ext).IsRequired();

        }

    }
}
