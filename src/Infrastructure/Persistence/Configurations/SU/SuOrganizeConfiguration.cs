using Domain.Entities.SU;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.SU;

public class SuOrganizeConfiguration : IEntityTypeConfiguration<SuOrganize>
{
    public void Configure(EntityTypeBuilder<SuOrganize> builder)
    {
        builder.HasKey(e => e.OrgId);

        builder.Property(e => e.OrgId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.OrgCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.OrgName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(e => e.OrgCode)
            .IsUnique();
    }
}
