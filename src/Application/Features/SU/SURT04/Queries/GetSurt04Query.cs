using Application.Common.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Models;

namespace Application.Features.SU.SURT04.Queries;

// 1. Get List
public record GetSurt04Query : IRequest<PaginatedList<Surt04Dto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; }
}

public class GetSurt04QueryHandler : IRequestHandler<GetSurt04Query, PaginatedList<Surt04Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSurt04QueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<Surt04Dto>> Handle(GetSurt04Query request, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .AsNoTracking()
            .Include(u => u.Employee)
            .Include(u => u.UserProfiles).ThenInclude(up => up.Profile)
            .Include(u => u.UserOrgs).ThenInclude(uo => uo.Org)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = request.SortDirection?.ToLower() == "desc" 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortColumn))
                : query.OrderBy(e => EF.Property<object>(e, request.SortColumn));
        }
        else
        {
            query = query.OrderBy(u => u.Username);
        }

        return await query
            .ProjectTo<Surt04Dto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
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
