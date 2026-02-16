using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.SU.Menus.Queries.GetMenus;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/menus")]
public class MenusController : ControllerBase
{
    private readonly ISender _sender;

    public MenusController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("current-user")]
    public async Task<ActionResult<List<MenuDto>>> GetCurrentUserMenus()
    {
        return await _sender.Send(new GetMenusQuery());
    }
}
