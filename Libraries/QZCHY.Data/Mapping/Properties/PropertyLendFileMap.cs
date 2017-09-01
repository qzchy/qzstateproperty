using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyLendFileMap : EntityTypeConfiguration<PropertyLendFile>
    {
        public PropertyLendFileMap()
        {
            this.ToTable("PropertyLend_File_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.File)
                .WithMany()
                .HasForeignKey(pp => pp.FileId);

            this.HasRequired(pp => pp.PropertyLend)
                .WithMany(p => p.LendFiles)
                .HasForeignKey(pp => pp.PropertyLendId);
        }
    }
}
