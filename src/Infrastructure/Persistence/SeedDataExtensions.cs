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

        // 1. Check if core data exists (Organizes)
        if (!await db.Organizes.AnyAsync())
        {
            logger.LogInformation("Seeding core development data...");

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
        }

        // 6. Menus (SuMenu) - UPSERT Logic
        logger.LogInformation("Seeding menu data...");

        var menus = new List<SuMenu>
        {
            new SuMenu { MenuCode = "DASHBOARD", MenuName = "Dashboard", Route = "/dashboard", Icon = "bi bi-speedometer2", Sequence = 1 },
            new SuMenu { MenuCode = "MANAGEMENT", MenuName = "Management", Sequence = 2 },
            new SuMenu { MenuCode = "SYSTEM", MenuName = "System Setup", Sequence = 4, Icon = "bi bi-gear" }
        };

        foreach (var m in menus)
        {
            var existing = await db.Menus.FirstOrDefaultAsync(x => x.MenuCode == m.MenuCode);
            if (existing == null)
            {
                await db.Menus.AddAsync(m);
            }
            else
            {
                // Update properties if needed
                existing.MenuName = m.MenuName;
                existing.Route = m.Route;
                existing.Icon = m.Icon;
                existing.Sequence = m.Sequence;
            }
        }
        await db.SaveChangesAsync();

        // Refetch to get IDs
        var dashboard = await db.Menus.FirstAsync(m => m.MenuCode == "DASHBOARD");
        var management = await db.Menus.FirstAsync(m => m.MenuCode == "MANAGEMENT");
        var system = await db.Menus.FirstAsync(m => m.MenuCode == "SYSTEM");

        // Sub Menus
        var subMenus = new List<SuMenu>
        {
            new SuMenu { MenuCode = "DBRT01", MenuName = "Employees", Route = "/db/dbrt01", Icon = "bi bi-people", Sequence = 3, ParentMenuId = management.MenuId },
            new SuMenu { MenuCode = "SURT01", MenuName = "Profile Setup", Route = "/su/surt01", Sequence = 5, ParentMenuId = system.MenuId },
            new SuMenu { MenuCode = "SURT02", MenuName = "Menu Setup", Route = "/su/surt02", Sequence = 5, ParentMenuId = system.MenuId },
            new SuMenu { MenuCode = "SURT03", MenuName = "Permission Setup", Route = "/su/surt03", Sequence = 5, ParentMenuId = system.MenuId },
            new SuMenu { MenuCode = "SURT04", MenuName = "User Management", Route = "/su/surt04", Sequence = 5, ParentMenuId = system.MenuId },
            new SuMenu { MenuCode = "SURT05", MenuName = "Audit Trails", Route = "/su/surt05", Sequence = 6, ParentMenuId = system.MenuId },
            new SuMenu { MenuCode = "SURT06", MenuName = "Organization Setup", Route = "/su/surt06", Sequence = 7, ParentMenuId = system.MenuId },
            new SuMenu { MenuCode = "SURT07", MenuName = "System Configs", Route = "/su/surt07", Sequence = 8, ParentMenuId = system.MenuId }
        };

        foreach (var sm in subMenus)
        {
            var existingSub = await db.Menus.FirstOrDefaultAsync(x => x.MenuCode == sm.MenuCode);
            if (existingSub == null)
            {
                await db.Menus.AddAsync(sm);
            }
            else
            {
                existingSub.MenuName = sm.MenuName;
                existingSub.Route = sm.Route;
                existingSub.Icon = sm.Icon;
                existingSub.Sequence = sm.Sequence;
                existingSub.ParentMenuId = sm.ParentMenuId;
            }
        }
        await db.SaveChangesAsync();

        // 7. Profile Menus (SuProfileMenu)
        // Grant Admin full access to all menus
        var dbAdminProfile = await db.Profiles.FirstOrDefaultAsync(p => p.ProfileCode == "ADMIN");
        if (dbAdminProfile != null)
        {
            var allMenus = await db.Menus.ToListAsync();
            foreach (var menu in allMenus)
            {
                var exists = await db.ProfileMenus.AnyAsync(pm => pm.ProfileId == dbAdminProfile.ProfileId && pm.MenuId == menu.MenuId);
                if (!exists)
                {
                    db.ProfileMenus.Add(new SuProfileMenu
                    {
                        ProfileId = dbAdminProfile.ProfileId,
                        MenuId = menu.MenuId,
                        CanView = true,
                        CanCreate = true,
                        CanEdit = true,
                        CanDelete = true
                    });
                }
            }
            await db.SaveChangesAsync();
        }


        // 8. System Configs (SuConfig)
        if (!await db.Configs.AnyAsync())
        {
            var configs = new List<SuConfig>
            {
                new SuConfig { ConfigKey = "MAX_UPLOAD_MB", ConfigValue = "10", Description = "Maximum allowed file upload size in MB", DataType = "number" },
                new SuConfig { ConfigKey = "ALLOW_GUEST_REGISTRATION", ConfigValue = "false", Description = "Allow new users to register themselves", DataType = "boolean" },
                new SuConfig { ConfigKey = "SYSTEM_NOTICE", ConfigValue = "", Description = "Global message displayed on the login page", DataType = "text" }
            };
            await db.Configs.AddRangeAsync(configs);
            await db.SaveChangesAsync();
        }

        logger.LogInformation("Development seed data created successfully.");
    }

    private static string GenerateSecurityStamp()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
}
