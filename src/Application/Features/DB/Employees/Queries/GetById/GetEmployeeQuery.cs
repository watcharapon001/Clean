using Application.Common.Abstractions;
using Application.Features.DB.Employees.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.Employees.Queries.GetById;

public record GetEmployeeQuery(string Id) : IRequest<EmployeeDto>;

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetEmployeeQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        
        var employee = await _context.DbEmployees
            .FirstOrDefaultAsync(e => e.EmployeeId.ToString() == request.Id && e.OrgId.ToString() == orgId, cancellationToken);

        if (employee == null)
             throw new KeyNotFoundException("Employee not found");

        return new EmployeeDto(
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
