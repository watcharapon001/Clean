using Application.Common.Abstractions;
using MediatR;

namespace Application.Features.SU.SURT06.Commands;

public record UpdateOrganizeCommand(
    Guid OrgId,
    string OrgName,
    bool IsActive
) : IRequest;

public class UpdateOrganizeCommandHandler : IRequestHandler<UpdateOrganizeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrganizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateOrganizeCommand request, CancellationToken cancellationToken)
    {
        var organize = await _context.Organizes.FindAsync(new object[] { request.OrgId }, cancellationToken);

        if (organize == null)
            throw new KeyNotFoundException($"Organization with ID {request.OrgId} was not found.");

        organize.OrgName = request.OrgName;
        organize.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
