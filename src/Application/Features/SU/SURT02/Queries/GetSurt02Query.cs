using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Models;

namespace Application.Features.SU.SURT02.Queries;

// 1. Get List
public record GetSurt02Query : IRequest<PaginatedList<Surt02Dto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; }
}

public class GetSurt02QueryHandler : IRequestHandler<GetSurt02Query, PaginatedList<Surt02Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt02QueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<Surt02Dto>> Handle(GetSurt02Query request, CancellationToken cancellationToken)
    {
        var query = _context.Menus
            .AsNoTracking()
            .Include(m => m.ParentMenu)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortColumn))
                : query.OrderBy(e => EF.Property<object>(e, request.SortColumn));
        }
        else
        {
            query = query.OrderBy(m => m.Sequence);
        }

        return await query
            .ProjectTo<Surt02Dto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
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
