using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT04.Queries;

// 1. Get List
public record GetSurt04Query : IRequest<List<Surt04Dto>>;

public class GetSurt04QueryHandler : IRequestHandler<GetSurt04Query, List<Surt04Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt04QueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Surt04Dto>> Handle(GetSurt04Query request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Employee)
            .Include(u => u.UserProfiles).ThenInclude(up => up.Profile)
            .Include(u => u.UserOrgs).ThenInclude(uo => uo.Org)
            .OrderBy(u => u.Username)
            .ProjectTo<Surt04Dto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

// 2. Get Detail
public record GetSurt04DetailQuery(Guid Id) : IRequest<Surt04Dto?>;

public class GetSurt04DetailQueryHandler : IRequestHandler<GetSurt04DetailQuery, Surt04Dto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt04DetailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Surt04Dto?> Handle(GetSurt04DetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Employee)
            .Include(u => u.UserProfiles).ThenInclude(up => up.Profile)
            .Include(u => u.UserOrgs).ThenInclude(uo => uo.Org)
            .Where(u => u.UserId == request.Id)
            .ProjectTo<Surt04Dto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
