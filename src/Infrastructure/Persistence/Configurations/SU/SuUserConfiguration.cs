using Domain.Entities.SU;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.SU;

public class SuUserConfiguration : IEntityTypeConfiguration<SuUser>
{
    public void Configure(EntityTypeBuilder<SuUser> builder)
    {
        builder.HasKey(e => e.UserId);

        builder.Property(e => e.UserId)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.Username)
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasMaxLength(320);

        builder.Property(e => e.EmailNormalized)
            .HasMaxLength(320);

        builder.Property(e => e.PasswordHash)
            .IsRequired();

        builder.Property(e => e.SecurityStamp)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.IsLocked)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.AccessFailedCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.AccessFailedCount)
            .HasDefaultValue(0)
            .IsRequired();

        // Unique: employee_id (1 employee = 1 user)
        builder.HasIndex(e => e.EmployeeId)
            .IsUnique()
            .HasFilter("employee_id IS NOT NULL");

        // Partial unique: email_normalized (WHERE NOT NULL)
        builder.HasIndex(e => e.EmailNormalized)
            .IsUnique()
            .HasFilter("email_normalized IS NOT NULL");

        // Partial unique: username (WHERE NOT NULL)
        builder.HasIndex(e => e.Username)
            .IsUnique()
            .HasFilter("username IS NOT NULL");

        // Index
        builder.HasIndex(e => e.IsActive);

        // FK → db_employee (one-to-one)
        builder.HasOne(e => e.Employee)
            .WithOne(emp => emp.User)
            .HasForeignKey<SuUser>(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
