using Application.Common.Abstractions.SU;
using Application.Common.Abstractions.DB;
using Domain.Entities.SU;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions;

public interface IApplicationDbContext : ISystemDbContext, IBusinessDbContext
{
    DbSet<DbEmployee> DbEmployees { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
