using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT02.Queries;

// 1. Get List
public record GetSurt02Query : IRequest<List<Surt02Dto>>;

public class GetSurt02QueryHandler : IRequestHandler<GetSurt02Query, List<Surt02Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt02QueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Surt02Dto>> Handle(GetSurt02Query request, CancellationToken cancellationToken)
    {
        return await _context.Menus
            .AsNoTracking()
            .Include(m => m.ParentMenu)
            .OrderBy(m => m.Sequence)
            .ProjectTo<Surt02Dto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

// 2. Get Detail
public record GetSurt02DetailQuery(Guid Id) : IRequest<Surt02Dto?>;

public class GetSurt02DetailQueryHandler : IRequestHandler<GetSurt02DetailQuery, Surt02Dto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt02DetailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Surt02Dto?> Handle(GetSurt02DetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Menus
            .AsNoTracking()
            .Where(m => m.MenuId == request.Id)
            .ProjectTo<Surt02Dto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
