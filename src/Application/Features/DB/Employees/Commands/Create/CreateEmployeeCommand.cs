using Application.Common.Abstractions;
using Domain.Entities.DB;
using MediatR;

namespace Application.Features.DB.Employees.Commands.Create;

public record CreateEmployeeCommand(
    string EmployeeCode,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    string? Email,
    string? Phone,
    bool IsActive
) : IRequest<string>;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        if (string.IsNullOrEmpty(orgId)) throw new UnauthorizedAccessException("Org context missing");

        var entity = new DbEmployee
        {
            EmployeeId = Guid.NewGuid(),
            OrgId = Guid.Parse(orgId),
            EmployeeCode = request.EmployeeCode,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = request.DisplayName,
            Email = request.Email,
            Phone = request.Phone,
            IsActive = request.IsActive
        };

        _context.DbEmployees.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.EmployeeId.ToString();
    }
}
