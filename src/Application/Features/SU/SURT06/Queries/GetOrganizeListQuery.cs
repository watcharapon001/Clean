using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Models;

namespace Application.Features.SU.SURT06.Queries;

public record OrganizeDto(
    Guid OrgId,
    string OrgCode,
    string OrgName,
    bool IsActive
);

public record GetOrganizeListQuery : IRequest<PaginatedList<OrganizeDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; }
}

public class GetOrganizeListQueryHandler : IRequestHandler<GetOrganizeListQuery, PaginatedList<OrganizeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrganizeListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<OrganizeDto>> Handle(GetOrganizeListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Organizes.AsNoTracking();

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = request.SortDirection?.ToLower() == "desc" 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortColumn))
                : query.OrderBy(e => EF.Property<object>(e, request.SortColumn));
        }
        else
        {
            query = query.OrderBy(o => o.OrgCode);
        }

        return await query
            .Select(o => new OrganizeDto(
                o.OrgId,
                o.OrgCode,
                o.OrgName,
                o.IsActive
            ))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
