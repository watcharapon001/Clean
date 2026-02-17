using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT04.Queries.GetOrganizes;

public record GetOrganizesQuery : IRequest<List<OrganizeDto>>;

public class GetOrganizesQueryHandler : IRequestHandler<GetOrganizesQuery, List<OrganizeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrganizesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrganizeDto>> Handle(GetOrganizesQuery request, CancellationToken cancellationToken)
    {
        return await _context.SuOrganizes
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ProjectTo<OrganizeDto>(_mapper.ConfigurationProvider)
            .OrderBy(t => t.OrgCode)
            .ToListAsync(cancellationToken);
    }
}
