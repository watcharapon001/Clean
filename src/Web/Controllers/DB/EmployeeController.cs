using Application.Features.DB.Employees.Commands.Create;
using Application.Features.DB.Employees.Commands.Delete;
using Application.Features.DB.Employees.Commands.Update;
using Application.Features.DB.Employees.Queries;
using Application.Features.DB.Employees.Queries.GetById;
using Application.Features.DB.Employees.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.DB;

[Authorize]
[ApiController]
[Route("api/employees")] // Matches frontend service
public class EmployeeController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetList()
    {
        return await _sender.Send(new GetEmployeesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> Get(string id)
    {
        return await _sender.Send(new GetEmployeeQuery(id));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateEmployeeCommand command)
    {
        return await _sender.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateEmployeeCommand command)
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
        await _sender.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}
