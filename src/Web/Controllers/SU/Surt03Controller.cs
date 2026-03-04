using Application.Features.SU.SURT03;
using Application.Features.SU.SURT03.Commands;
using Application.Features.SU.SURT03.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt03")]
public class Surt03Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt03Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{profileId}")]
    public async Task<ActionResult<List<Surt03Dto>>> Get(Guid profileId)
    {
        return await _sender.Send(new GetSurt03Query(profileId));
    }

    [HttpGet("export/{profileId}")]
    public async Task<IActionResult> Export(Guid profileId)
    {
        var fileBytes = await _sender.Send(new ExportSurt03Query(profileId));
        var fileName = $"Permissions_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    [HttpPut("{profileId}")]
    public async Task<ActionResult> Update(Guid profileId, UpdateSurt03Command command)
    {
        if (profileId != command.ProfileId) return BadRequest();
        await _sender.Send(command);
        return NoContent();
    }
}
