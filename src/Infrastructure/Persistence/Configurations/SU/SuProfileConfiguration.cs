using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.SU;

public class SuProfileConfiguration : IEntityTypeConfiguration<SuProfile>
{
    public void Configure(EntityTypeBuilder<SuProfile> builder)
    {
        builder.HasKey(e => e.ProfileId);

        builder.Property(e => e.ProfileId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.ProfileCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.ProfileName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(e => e.ProfileCode)
            .IsUnique();
    }
}
