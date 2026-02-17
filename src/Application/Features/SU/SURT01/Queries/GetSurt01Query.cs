using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT01.Queries;

// 1. Get List
public record GetSurt01Query : IRequest<List<Surt01Dto>>;

public class GetSurt01QueryHandler : IRequestHandler<GetSurt01Query, List<Surt01Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt01QueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Surt01Dto>> Handle(GetSurt01Query request, CancellationToken cancellationToken)
    {
        return await _context.Profiles
            .AsNoTracking()
            .OrderBy(p => p.ProfileCode)
            .ProjectTo<Surt01Dto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

// 2. Get Detail
public record GetSurt01DetailQuery(Guid Id) : IRequest<Surt01Dto?>;

public class GetSurt01DetailQueryHandler : IRequestHandler<GetSurt01DetailQuery, Surt01Dto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt01DetailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Surt01Dto?> Handle(GetSurt01DetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Profiles
            .AsNoTracking()
            .Where(p => p.ProfileId == request.Id)
            .ProjectTo<Surt01Dto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
