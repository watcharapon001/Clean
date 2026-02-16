using Application.Features.SU.SURT01;
using Application.Features.SU.SURT01.Commands;
using Application.Features.SU.SURT01.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/surt01")]
public class Surt01Controller : ControllerBase
{
    private readonly ISender _sender;

    public Surt01Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProfileDto>>> GetList()
    {
        return await _sender.Send(new GetSurt01Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDto>> Get(Guid id)
    {
        var result = await _sender.Send(new GetSurt01DetailQuery(id));
        if (result == null) return NotFound();
        return result;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateSurt01Command command)
    {
        return await _sender.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateSurt01Command command)
    {
        if (id != command.ProfileId) return BadRequest();
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteSurt01Command(id));
        return NoContent();
    }
}
