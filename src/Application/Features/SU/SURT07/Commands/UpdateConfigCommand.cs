using Application.Common.Abstractions;
using MediatR;

namespace Application.Features.SU.SURT07.Commands;

public record UpdateConfigCommand(
    string ConfigKey,
    string ConfigValue
) : IRequest;

public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateConfigCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
    {
        var config = await _context.SuConfigs.FindAsync(new object[] { request.ConfigKey }, cancellationToken);

        if (config == null)
            throw new KeyNotFoundException($"ConfigKey '{request.ConfigKey}' was not found.");

        config.ConfigValue = request.ConfigValue;
        // Description and DataType are effectively read-only/managed by developers
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}
