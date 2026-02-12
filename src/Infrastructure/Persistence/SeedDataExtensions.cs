using System.Security.Cryptography;
using Domain.Entities.SU;
using Domain.Entities.DB;
using Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public static class SeedDataExtensions
{
    public static async Task SeedDevelopmentDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        // Ensure schema & tables exist
        await db.Database.MigrateAsync();

        // Check if seeded, but also ensure Admin password is correct (Fix for login issue)
        var existingAdmin = await db.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (existingAdmin != null)
        {
             var resetHasher = new PasswordHasher<SuUser>();
             var newHash = resetHasher.HashPassword(existingAdmin, "Admin123!");
             
             // Only update if different (optional, but good for perf) - actually just force it to be sure
             existingAdmin.PasswordHash = newHash;
             existingAdmin.SecurityStamp = GenerateSecurityStamp(); // Invalidate old sessions
             
             db.Users.Update(existingAdmin);
             await db.SaveChangesAsync();
             logger.LogInformation("Admin password reset to 'Admin123!'.");
        }

        // Skip if already seeded (Organizes check from before)
        if (await db.Organizes.AnyAsync())
        {
            logger.LogInformation("Database already seeded. Skipping creation.");
            return;
        }

        logger.LogInformation("Seeding development data...");

        // 1. Organizes (SuOrganize)
        var org1 = new SuOrganize { OrgCode = "ORG01", OrgName = "Headquarters" };
        var org2 = new SuOrganize { OrgCode = "ORG02", OrgName = "Branch A" };
        await db.Organizes.AddRangeAsync(org1, org2);

        // 2. Profiles (SuProfile)
        var adminProfile = new SuProfile { ProfileCode = "ADMIN", ProfileName = "Administrator" };
        var userProfile = new SuProfile { ProfileCode = "USER", ProfileName = "Standard User" };
        await db.Profiles.AddRangeAsync(adminProfile, userProfile);

        await db.SaveChangesAsync(); // Save to get IDs

        // 3. Employees (DbEmployee)
        var emp1 = new DbEmployee
        {
            OrgId = org1.OrgId,
            EmployeeCode = "EMP001",
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };
        var emp2 = new DbEmployee
        {
            OrgId = org1.OrgId,
            EmployeeCode = "EMP002",
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com"
        };
        var emp3 = new DbEmployee
        {
            OrgId = org2.OrgId,
            EmployeeCode = "EMP003",
            FirstName = "Peter",
            LastName = "Jones",
            Email = "peter@example.com"
        };
        await db.Employees.AddRangeAsync(emp1, emp2, emp3);
        await db.SaveChangesAsync();

        // 4. Users (SuUser)
        var hasher = new PasswordHasher<SuUser>();

        var user1 = new SuUser
        {
            EmployeeId = emp1.EmployeeId,
            Username = "admin",
            Email = emp1.Email,
            EmailNormalized = emp1.Email!.ToUpper(),
            IsActive = true,
            SecurityStamp = GenerateSecurityStamp(),
        };
        user1.PasswordHash = hasher.HashPassword(user1, "Admin123!");

        var user2 = new SuUser
        {
            EmployeeId = emp2.EmployeeId,
            Username = "jane",
            Email = emp2.Email,
            EmailNormalized = emp2.Email!.ToUpper(),
            IsActive = true,
            SecurityStamp = GenerateSecurityStamp(),
        };
        user2.PasswordHash = hasher.HashPassword(user2, "User123!");

        await db.Users.AddRangeAsync(user1, user2);
        await db.SaveChangesAsync();

        // 5. Links (SuUserOrg, SuUserProfile)
        // Link Admin to both orgs, User to Org1
        await db.UserOrgs.AddRangeAsync(
            new SuUserOrg { UserId = user1.UserId, OrgId = org1.OrgId, IsDefault = true },
            new SuUserOrg { UserId = user1.UserId, OrgId = org2.OrgId, IsDefault = false },
            new SuUserOrg { UserId = user2.UserId, OrgId = org1.OrgId, IsDefault = true }
        );

        // Link Admin to ADMIN, User to USER
        await db.UserProfiles.AddRangeAsync(
            new SuUserProfile { UserId = user1.UserId, ProfileId = adminProfile.ProfileId },
            new SuUserProfile { UserId = user2.UserId, ProfileId = userProfile.ProfileId }
        );

        await db.SaveChangesAsync();
        logger.LogInformation("Development seed data created successfully.");
    }

    private static string GenerateSecurityStamp()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
}
