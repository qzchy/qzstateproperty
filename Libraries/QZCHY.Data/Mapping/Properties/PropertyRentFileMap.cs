using QZCHY.Core.Domain.Properties;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyRentFileMap : EntityTypeConfiguration<PropertyRentFile>
    {
        public PropertyRentFileMap()
        {
            this.ToTable("PropertyRent_File_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.File)
                .WithMany()
                .HasForeignKey(pp => pp.FileId);

            this.HasRequired(pp => pp.PropertyRent)
                .WithMany(p => p.RentFiles)
                .HasForeignKey(pp => pp.PropertyRentId);
        }
    }
}
