using QZCHY.Core.Domain.Properties;

namespace QZCHY.Data.Mapping.Properties
{
    public class GovernmentUnitMap : EntityTypeConfiguration<GovernmentUnit>
    {
        public GovernmentUnitMap()
        {
            this.ToTable("GovernmentUnit");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(255);
            this.Property(p => p.Person).HasMaxLength(255);
            this.Property(p => p.Tel).HasMaxLength(255); 
        }
    }
}
