using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions.DB;

public interface IBusinessDbContext
{
    DbSet<DbEmployee> Employees { get; }
}
