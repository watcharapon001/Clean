using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT06.Commands;

public record DeleteOrganizeCommand(Guid OrgId) : IRequest;

public class DeleteOrganizeCommandHandler : IRequestHandler<DeleteOrganizeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteOrganizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteOrganizeCommand request, CancellationToken cancellationToken)
    {
        var organize = await _context.Organizes.FindAsync(new object[] { request.OrgId }, cancellationToken);

        if (organize == null)
            throw new KeyNotFoundException($"Organization with ID {request.OrgId} was not found.");

        // Check for related data (Users attached to this org)
        var hasUsers = await _context.UserOrgs.AnyAsync(uo => uo.OrgId == request.OrgId, cancellationToken);
        if (hasUsers)
            throw new InvalidOperationException("Cannot delete an organization that has active users assigned. Disable it instead.");

        _context.Organizes.Remove(organize);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
