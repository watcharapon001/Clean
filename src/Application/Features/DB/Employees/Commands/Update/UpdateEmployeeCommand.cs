using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.Employees.Commands.Update;

public record UpdateEmployeeCommand(
    string EmployeeId,
    string EmployeeCode,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    string? Email,
    string? Phone,
    bool IsActive
) : IRequest;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        
        var entity = await _context.DbEmployees
            .FirstOrDefaultAsync(e => e.EmployeeId.ToString() == request.EmployeeId && e.OrgId.ToString() == orgId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("Employee not found");

        entity.EmployeeCode = request.EmployeeCode;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.DisplayName = request.DisplayName;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
