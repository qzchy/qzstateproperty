using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyFileMap : EntityTypeConfiguration<PropertyFile>
    {
        public PropertyFileMap()
        {
            this.ToTable("Property_File_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.File)
                .WithMany()
                .HasForeignKey(pp => pp.FileId);

            this.HasRequired(pp => pp.Property)
                .WithMany(p => p.Files)
                .HasForeignKey(pp => pp.PropertyId);
        }
    }
}
