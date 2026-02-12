using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions.SU;

public interface ISystemDbContext
{
    DbSet<SuUser> Users { get; }
    DbSet<SuOrganize> Organizes { get; }
    DbSet<SuUserOrg> UserOrgs { get; }
    DbSet<SuProfile> Profiles { get; }
    DbSet<SuUserProfile> UserProfiles { get; }
}
