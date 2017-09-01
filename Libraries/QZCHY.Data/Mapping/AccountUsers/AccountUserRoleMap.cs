using QZCHY.Core.Domain.AccountUsers;

namespace QZCHY.Data.Mapping.AccountUsers
{
    public partial class AccountUserRoleMap : EntityTypeConfiguration<AccountUserRole>
    {
        public AccountUserRoleMap()
        {
            this.ToTable("AccountUserRole");
            this.HasKey(cr => cr.Id);
            this.Property(cr => cr.Name).IsRequired().HasMaxLength(255);
            this.Property(cr => cr.SystemName).HasMaxLength(255);
        }
    }
}
