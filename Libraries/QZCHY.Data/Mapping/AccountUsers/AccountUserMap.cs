using QZCHY.Core.Domain.AccountUsers;

namespace QZCHY.Data.Mapping.AccountUsers
{
    public partial class AccountUserMap : EntityTypeConfiguration<AccountUser>
    {
        public AccountUserMap()
        {
            this.ToTable("AccountUser");
            this.HasKey(c => c.Id);
            this.Property(u => u.UserName).HasMaxLength(1000);
            this.Property(u => u.SystemName).HasMaxLength(400);

            this.Ignore(u => u.PasswordFormat);

            this.HasMany(c => c.AccountUserRoles)
                .WithMany()
                .Map(m => m.ToTable("AccountUser_AccountUserRole_Mapping"));
        }
    }
}
