using Application.Features.SU.SURT02;
using Application.Features.SU.SURT02.Commands;
using Application.Features.SU.SURT02.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt02")]
public class Surt02Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt02Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<List<Surt02Dto>>> GetList()
    {
        return await _sender.Send(new GetSurt02Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Surt02Dto>> Get(Guid id)
    {
        var result = await _sender.Send(new GetSurt02DetailQuery(id));
        if (result == null) return NotFound();
        return result;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateSurt02Command command)
    {
        return await _sender.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateSurt02Command command)
    {
        if (id != command.MenuId) return BadRequest();
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteSurt02Command(id));
        return NoContent();
    }
}
