using System.Reflection;
using Application.Common.Abstractions;
using Domain.Entities.SU;
using Domain.Entities.DB;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts;

public partial class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }



    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Domain.Common.BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreateDate = DateTimeOffset.UtcNow;
                    // entry.Entity.CreateBy = _currentUserService.UserId; // TODO: Implement Auth
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateDate = DateTimeOffset.UtcNow;
                    // entry.Entity.UpdateBy = _currentUserService.UserId; // TODO: Implement Auth
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Default schema
        modelBuilder.HasDefaultSchema("clean");

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Global Snake Case Convention
        modelBuilder.ApplySnakeCaseNamingConvention();

        base.OnModelCreating(modelBuilder);
    }
}
