using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.SU;

public class SuUserOrgConfiguration : IEntityTypeConfiguration<SuUserOrg>
{
    public void Configure(EntityTypeBuilder<SuUserOrg> builder)
    {
        // Composite PK
        builder.HasKey(e => new { e.UserId, e.OrgId });

        builder.Property(e => e.IsDefault)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        // Partial unique: 1 default per user
        builder.HasIndex(e => e.UserId)
            .IsUnique()
            .HasFilter("is_default = true")
            .HasDatabaseName("ix_su_user_org_default_per_user");

        // Index
        builder.HasIndex(e => e.OrgId);

        // FK → su_user
        builder.HasOne(e => e.User)
            .WithMany(u => u.UserOrgs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK → su_organize
        builder.HasOne(e => e.Org)
            .WithMany(o => o.UserOrgs)
            .HasForeignKey(e => e.OrgId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
