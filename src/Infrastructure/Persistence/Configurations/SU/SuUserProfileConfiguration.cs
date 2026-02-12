using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.SU;

public class SuUserProfileConfiguration : IEntityTypeConfiguration<SuUserProfile>
{
    public void Configure(EntityTypeBuilder<SuUserProfile> builder)
    {
        // Composite PK
        builder.HasKey(e => new { e.UserId, e.ProfileId });

        // Index
        builder.HasIndex(e => e.ProfileId);

        // FK → su_user
        builder.HasOne(e => e.User)
            .WithMany(u => u.UserProfiles)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK → su_profile
        builder.HasOne(e => e.Profile)
            .WithMany(p => p.UserProfiles)
            .HasForeignKey(e => e.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
