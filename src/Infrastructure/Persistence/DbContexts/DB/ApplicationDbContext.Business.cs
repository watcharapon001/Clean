using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts;

public partial class ApplicationDbContext
{
    public DbSet<DbEmployee> Employees => Set<DbEmployee>();
}
