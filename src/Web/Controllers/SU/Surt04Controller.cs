using Application.Features.SU.SURT04;
using Application.Features.SU.SURT04.Commands;
using Application.Features.SU.SURT04.Queries;
using Application.Features.SU.SURT04.Queries.GetOrganizes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt04")]
public class Surt04Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt04Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<List<Surt04Dto>>> GetList()
    {
        return await _sender.Send(new GetSurt04Query());
    }

    [HttpGet("organizes")]
    public async Task<ActionResult<List<OrganizeDto>>> GetOrganizes()
    {
        return await _sender.Send(new GetOrganizesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Surt04Dto>> Get(Guid id)
    {
        var result = await _sender.Send(new GetSurt04DetailQuery(id));
        if (result == null) return NotFound();
        return result;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateSurt04Command command)
    {
        return await _sender.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateSurt04Command command)
    {
        if (id != command.UserId) return BadRequest();
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteSurt04Command(id));
        return NoContent();
    }
}
