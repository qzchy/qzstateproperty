using QZCHY.Core.Domain.Properties;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyOffFileMap : EntityTypeConfiguration<PropertyOffFile>
    {
        public PropertyOffFileMap()
        {
            this.ToTable("PropertyOff_File_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.File)
                .WithMany()
                .HasForeignKey(pp => pp.FileId);

            this.HasRequired(pp => pp.PropertyOff)
                .WithMany(p => p.OffFiles)
                .HasForeignKey(pp => pp.PropertyOffId);
        }
    }
}
