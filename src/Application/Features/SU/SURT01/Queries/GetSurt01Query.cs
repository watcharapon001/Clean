using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Models;

namespace Application.Features.SU.SURT01.Queries;

// 1. Get List (Paginated)
public record GetSurt01Query : IRequest<PaginatedList<Surt01Dto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } 
    public string? SortDirection { get; init; }
}

public class GetSurt01QueryHandler : IRequestHandler<GetSurt01Query, PaginatedList<Surt01Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt01QueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<Surt01Dto>> Handle(GetSurt01Query request, CancellationToken cancellationToken)
    {
        var query = _context.Profiles.AsNoTracking();

        // Optional Simple Sorting (can be expanded later dynamically)
        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = request.SortDirection?.ToLower() == "desc" 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortColumn))
                : query.OrderBy(e => EF.Property<object>(e, request.SortColumn));
        }
        else
        {
            query = query.OrderBy(p => p.ProfileCode);
        }

        return await query
            .ProjectTo<Surt01Dto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
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
