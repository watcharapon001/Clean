using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions.SU;

public interface ISystemDbContext
{
    public DbSet<SuUser> Users { get; }
    public DbSet<SuOrganize> Organizes { get; }
    public DbSet<SuUserOrg> UserOrgs { get; }
    public DbSet<SuProfile> Profiles { get; }
    public DbSet<SuUserProfile> UserProfiles { get; }
    public DbSet<SuMenu> Menus { get; }
    public DbSet<SuProfileMenu> ProfileMenus { get; }
    public DbSet<SuConfig> Configs { get; }
}
