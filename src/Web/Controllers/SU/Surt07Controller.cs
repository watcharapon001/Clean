using Application.Features.SU.SURT07.Commands;
using Application.Features.SU.SURT07.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt07")]
public class Surt07Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt07Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("list")]
    public async Task<ActionResult<Application.Common.Models.PaginatedList<ConfigDto>>> GetList([FromQuery] GetConfigListQuery query)
    {
        return await _sender.Send(query);
    }


    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateConfigCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }
}
