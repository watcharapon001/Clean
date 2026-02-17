using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts;

public partial class ApplicationDbContext
{
    public DbSet<SuUser> Users => Set<SuUser>();
    public DbSet<SuOrganize> Organizes => Set<SuOrganize>();
    public DbSet<SuUserOrg> UserOrgs => Set<SuUserOrg>();
    public DbSet<SuProfile> Profiles => Set<SuProfile>();
    public DbSet<SuUserProfile> UserProfiles => Set<SuUserProfile>();
    public DbSet<SuMenu> Menus => Set<SuMenu>();
    public DbSet<SuProfileMenu> ProfileMenus => Set<SuProfileMenu>();
}
