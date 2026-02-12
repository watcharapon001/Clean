using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.DB;

public class DbEmployeeConfiguration : IEntityTypeConfiguration<DbEmployee>
{
    public void Configure(EntityTypeBuilder<DbEmployee> builder)
    {
        builder.HasKey(e => e.EmployeeId);

        builder.Property(e => e.EmployeeId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.OrgId)
            .IsRequired();

        builder.Property(e => e.EmployeeCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.FirstName)
            .HasMaxLength(200);

        builder.Property(e => e.LastName)
            .HasMaxLength(200);

        builder.Property(e => e.DisplayName)
            .HasMaxLength(200);

        builder.Property(e => e.Email)
            .HasMaxLength(320);

        builder.Property(e => e.Phone)
            .HasMaxLength(30);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        // Unique: (org_id, employee_code)
        builder.HasIndex(e => new { e.OrgId, e.EmployeeCode })
            .IsUnique();

        // Optional indexes
        builder.HasIndex(e => e.OrgId);
        builder.HasIndex(e => e.Email);

        // FK → su_organize
        builder.HasOne(e => e.Org)
            .WithMany(o => o.Employees)
            .HasForeignKey(e => e.OrgId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
