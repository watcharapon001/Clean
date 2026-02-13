using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Queries.GetMe;
using Application.Features.Auth.Queries.GetOrgs;
using Application.Features.Auth.Commands.SwitchOrg;
using Application.Features.Auth.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        return await _sender.Send(command);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        return await _sender.Send(new GetMeQuery());
    }

    [Authorize]
    [HttpGet("orgs")]
    public async Task<ActionResult<List<OrgDto>>> GetOrgs()
    {
        return await _sender.Send(new GetOrgsQuery());
    }

    [Authorize]
    [HttpPost("switch-org")]
    public async Task<ActionResult<LoginResponse>> SwitchOrg(SwitchOrgCommand command)
    {
        return await _sender.Send(command);
    }
}
