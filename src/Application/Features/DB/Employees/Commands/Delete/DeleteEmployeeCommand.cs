using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.Employees.Commands.Delete;

public record DeleteEmployeeCommand(string Id) : IRequest;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        
        var entity = await _context.DbEmployees
             .FirstOrDefaultAsync(e => e.EmployeeId.ToString() == request.Id && e.OrgId.ToString() == orgId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("Employee not found");

        _context.DbEmployees.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
