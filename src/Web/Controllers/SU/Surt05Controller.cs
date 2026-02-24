using Application.Features.SU.SURT05.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt05")]
public class Surt05Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt05Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("audit-logs")]
    public async Task<ActionResult<List<AuditLogDto>>> GetAuditLogs()
    {
        return await _sender.Send(new GetAuditLogsQuery());
    }
}
