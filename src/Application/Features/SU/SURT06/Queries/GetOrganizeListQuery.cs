using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT06.Queries;

public record OrganizeDto(
    Guid OrgId,
    string OrgCode,
    string OrgName,
    bool IsActive
);

public record GetOrganizeListQuery : IRequest<List<OrganizeDto>>;

public class GetOrganizeListQueryHandler : IRequestHandler<GetOrganizeListQuery, List<OrganizeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrganizeListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrganizeDto>> Handle(GetOrganizeListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Organizes
            .OrderBy(o => o.OrgCode)
            .Select(o => new OrganizeDto(
                o.OrgId,
                o.OrgCode,
                o.OrgName,
                o.IsActive
            ))
            .ToListAsync(cancellationToken);
    }
}
