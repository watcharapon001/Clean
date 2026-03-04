using Application.Features.DB.DBRT01;
using Application.Features.DB.DBRT01.Commands;
using Application.Features.DB.DBRT01.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.DB;

[Authorize]
[ApiController]
[Route("api/dbrt01")]
public class Dbrt01Controller : ControllerBase
{
    private readonly ISender _sender;

    public Dbrt01Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<Application.Common.Models.PaginatedList<Dbrt01Dto>>> GetList([FromQuery] GetDbrt01Query query)
    {
        return await _sender.Send(query);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Dbrt01Dto>> Get(string id)
    {
        return await _sender.Send(new GetDbrt01DetailQuery(id));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateDbrt01Command command)
    {
        return await _sender.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateDbrt01Command command)
    {
        if (id != command.EmployeeId)
        {
            return BadRequest();
        }
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _sender.Send(new DeleteDbrt01Command(id));
        return NoContent();
    }
}
