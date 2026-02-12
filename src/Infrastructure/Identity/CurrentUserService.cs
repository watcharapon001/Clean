using System.Security.Claims;
using Application.Common.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? OrgId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("org_id");
}
