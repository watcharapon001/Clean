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

        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is SuAuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Metadata.GetTableName() ?? entry.Metadata.Name;
            auditEntry.Action = entry.State.ToString();
            auditEntry.UserId = _currentUserService.UserId ?? "Unknown";

            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }

        foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
        {
            SuAuditLogs.Add(auditEntry.ToAudit());
        }

        var pendingAuditEntries = auditEntries.Where(_ => _.HasTemporaryProperties).ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (pendingAuditEntries.Any())
        {
            foreach (var auditEntry in pendingAuditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                SuAuditLogs.Add(auditEntry.ToAudit());
            }
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    public DbSet<SuOrganize> SuOrganizes => Set<SuOrganize>();
    public DbSet<DbEmployee> DbEmployees => Set<DbEmployee>();
    public DbSet<SuAuditLog> SuAuditLogs => Set<SuAuditLog>();
    public DbSet<SuConfig> SuConfigs => Set<SuConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Default schema
        modelBuilder.HasDefaultSchema("clean");

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Global Snake Case Convention
        modelBuilder.ApplySnakeCaseNamingConvention();

        // Composite key for SuProfileMenu
        modelBuilder.Entity<SuProfileMenu>()
            .HasKey(pm => new { pm.ProfileId, pm.MenuId });
            
        modelBuilder.Entity<SuMenu>()
            .HasKey(m => m.MenuId);

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
