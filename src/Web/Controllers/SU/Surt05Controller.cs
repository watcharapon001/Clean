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

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var fileBytes = await _sender.Send(new ExportAuditLogsQuery());
        var fileName = $"AuditLogs_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
