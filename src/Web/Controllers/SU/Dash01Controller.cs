using Application.Features.SU.DASH01.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU;

[ApiController]
[Route("api/su/dash01")]
public class Dash01Controller : ControllerBase
{
    private readonly ISender _sender;

    public Dash01Controller(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<DashboardMetricsDto>> GetMetrics()
    {
        return await _sender.Send(new GetDashboardMetricsQuery());
    }
}
