using Application.Common.Abstractions;
using Application.Features.DB.Employees.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.Employees.Queries.GetList;

public record GetEmployeesQuery : IRequest<List<EmployeeDto>>;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetEmployeesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        if (string.IsNullOrEmpty(orgId)) return new List<EmployeeDto>(); // Or throw

        var employees = await _context.DbEmployees
            .Where(e => e.OrgId.ToString() == orgId)
            .Select(e => new EmployeeDto(
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
            .ToListAsync(cancellationToken);

        return employees;
    }
}
