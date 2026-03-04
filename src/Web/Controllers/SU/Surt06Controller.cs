using Application.Features.SU.SURT06.Commands;
using Application.Features.SU.SURT06.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt06")]
public class Surt06Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt06Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("list")]
    public async Task<ActionResult<Application.Common.Models.PaginatedList<OrganizeDto>>> GetList([FromQuery] GetOrganizeListQuery query)
    {
        return await _sender.Send(query);
    }


    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(CreateOrganizeCommand command)
    {
        var orgId = await _sender.Send(command);
        return Ok(orgId);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateOrganizeCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteOrganizeCommand(id));
        return NoContent();
    }
}
