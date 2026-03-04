using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Models;

namespace Application.Features.DB.DBRT01.Queries;

// 1. Get List
public record GetDbrt01Query : IRequest<PaginatedList<Dbrt01Dto>>
{
    public bool OnlyAvailable { get; init; } = false;
    public Guid? IncludeUserId { get; init; } = null;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; }
}

public class GetDbrt01QueryHandler : IRequestHandler<GetDbrt01Query, PaginatedList<Dbrt01Dto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetDbrt01QueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<Dbrt01Dto>> Handle(GetDbrt01Query request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        if (string.IsNullOrEmpty(orgId)) 
            return new PaginatedList<Dbrt01Dto>(new List<Dbrt01Dto>(), 0, request.PageNumber, request.PageSize); 

        var query = _context.DbEmployees
            .AsNoTracking()
            .Where(e => e.OrgId.ToString() == orgId);

        if (request.OnlyAvailable)
        {
            query = query.Where(e => e.User == null || (request.IncludeUserId.HasValue && e.User.UserId == request.IncludeUserId.Value));
        }

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = request.SortDirection?.ToLower() == "desc" 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortColumn))
                : query.OrderBy(e => EF.Property<object>(e, request.SortColumn));
        }
        else
        {
            query = query.OrderBy(e => e.EmployeeCode);
        }

        return await query
            .Select(e => new Dbrt01Dto(
                e.EmployeeId.ToString(),
                e.OrgId.ToString(),
                e.EmployeeCode,
                e.FirstName,
                e.LastName,
                e.DisplayName,
                e.Email,
                e.Phone,
                e.IsActive
            ))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

// 2. Get Detail
public record GetDbrt01DetailQuery(string Id) : IRequest<Dbrt01Dto>;

public class GetDbrt01DetailQueryHandler : IRequestHandler<GetDbrt01DetailQuery, Dbrt01Dto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetDbrt01DetailQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Dbrt01Dto> Handle(GetDbrt01DetailQuery request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        
        var employee = await _context.DbEmployees
            .FirstOrDefaultAsync(e => e.EmployeeId.ToString() == request.Id && e.OrgId.ToString() == orgId, cancellationToken);

        if (employee == null)
             throw new KeyNotFoundException("Employee not found");

        return new Dbrt01Dto(
            employee.EmployeeId.ToString(),
            employee.OrgId.ToString(),
            employee.EmployeeCode,
            employee.FirstName,
            employee.LastName,
            employee.DisplayName,
            employee.Email,
            employee.Phone,
            employee.IsActive
        );
    }
}
