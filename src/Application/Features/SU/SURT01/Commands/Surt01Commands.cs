using Application.Common.Abstractions;
using Domain.Entities.SU;
using MediatR;

namespace Application.Features.SU.SURT01.Commands;

// 1. Create
public record CreateSurt01Command : IRequest<Guid>
{
    public string ProfileCode { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSurt01CommandHandler : IRequestHandler<CreateSurt01Command, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSurt01CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSurt01Command request, CancellationToken cancellationToken)
    {
        var entity = new SuProfile
        {
            ProfileId = Guid.NewGuid(),
            ProfileCode = request.ProfileCode,
            ProfileName = request.ProfileName,
            Description = request.Description,
            IsActive = request.IsActive
        };

        _context.Profiles.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.ProfileId;
    }
}

// 2. Update
public record UpdateSurt01Command : IRequest
{
    public Guid ProfileId { get; set; }
    public string ProfileCode { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateSurt01CommandHandler : IRequestHandler<UpdateSurt01Command>
{
    private readonly IApplicationDbContext _context;

    public UpdateSurt01CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSurt01Command request, CancellationToken cancellationToken)
    {
        var entity = await _context.Profiles.FindAsync(new object[] { request.ProfileId }, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity \"{nameof(SuProfile)}\" ({request.ProfileId}) was not found.");
        }

        entity.ProfileCode = request.ProfileCode;
        entity.ProfileName = request.ProfileName;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

// 3. Delete
public record DeleteSurt01Command(Guid Id) : IRequest;

public class DeleteSurt01CommandHandler : IRequestHandler<DeleteSurt01Command>
{
    private readonly IApplicationDbContext _context;

    public DeleteSurt01CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSurt01Command request, CancellationToken cancellationToken)
    {
        var entity = await _context.Profiles.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
             throw new Exception($"Entity \"{nameof(SuProfile)}\" ({request.Id}) was not found.");
        }

        _context.Profiles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
