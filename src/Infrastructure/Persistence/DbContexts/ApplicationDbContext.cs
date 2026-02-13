using System.Reflection;
using Application.Common.Abstractions;
using Domain.Entities.SU;
using Domain.Entities.DB;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts;

public partial class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public Guid? CurrentOrgId => Guid.TryParse(_currentUserService.OrgId, out var id) ? id : null;

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

        // Global Org Filter
        // We need to apply this to all entities that implement IOrgEntity.
        // Since we can't easily iterate and use generic HasQueryFilter with explicit lambda without reflection,
        // and current set is small, we can do it explicitly or use a helper.
        // For now, explicit for DbEmployee (and others if properly identified).
        // Or better: filter by interface.
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Domain.Common.IOrgEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(ConfigureOrgFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(entityType.ClrType);
                
                method?.Invoke(this, new object[] { modelBuilder });
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureOrgFilter<T>(ModelBuilder modelBuilder) where T : class, Domain.Common.IOrgEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => e.OrgId == CurrentOrgId);
    }
}
