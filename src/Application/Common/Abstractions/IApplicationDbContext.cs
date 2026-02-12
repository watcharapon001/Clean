using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions;

public interface IApplicationDbContext
{
    // Add DbSet properties here as entities are created
    // Example: DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
