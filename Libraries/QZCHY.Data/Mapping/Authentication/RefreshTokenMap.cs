using QZCHY.Core.Domain.Authentication;

namespace QZCHY.Data.Mapping.Authentication
{
    public class RefreshTokenMap : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenMap()
        {
            this.HasKey(tc => tc.Id);
        }
    }
}
