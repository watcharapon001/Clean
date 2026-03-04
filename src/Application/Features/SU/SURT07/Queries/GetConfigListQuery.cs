using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Models;

namespace Application.Features.SU.SURT07.Queries;

public record ConfigDto(
    string ConfigKey,
    string ConfigValue,
    string? Description,
    string DataType
);

public record GetConfigListQuery : IRequest<PaginatedList<ConfigDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; }
}

public class GetConfigListQueryHandler : IRequestHandler<GetConfigListQuery, PaginatedList<ConfigDto>>
{
    private readonly IApplicationDbContext _context;

    public GetConfigListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ConfigDto>> Handle(GetConfigListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SuConfigs.AsNoTracking();

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = request.SortDirection?.ToLower() == "desc" 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortColumn))
                : query.OrderBy(e => EF.Property<object>(e, request.SortColumn));
        }
        else
        {
            query = query.OrderBy(c => c.ConfigKey);
        }

        return await query
            .Select(c => new ConfigDto(
                c.ConfigKey,
                c.ConfigValue,
                c.Description,
                c.DataType
            ))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
