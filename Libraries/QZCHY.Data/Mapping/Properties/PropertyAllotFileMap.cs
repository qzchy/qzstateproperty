using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyAllotFileMap : EntityTypeConfiguration<PropertyAllotFile>
    {
        public PropertyAllotFileMap()
        {
            this.ToTable("PropertyAllot_File_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.File)
                .WithMany()
                .HasForeignKey(pp => pp.FileId);

            this.HasRequired(pp => pp.PropertyAllot)
                .WithMany(p => p.AllotFiles)
                .HasForeignKey(pp => pp.PropertyAllotId);
        }
    }
}
