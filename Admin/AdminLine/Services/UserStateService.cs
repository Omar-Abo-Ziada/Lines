using System.Security.Claims;
using Lines.Application.Interfaces;

namespace AdminLine.Services;

public class UserStateService : IUserStateService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserStateService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value is string id ? Guid.Parse(id) : Guid.Empty;

    public string? UserName => _httpContextAccessor.HttpContext?.User.Identity?.Name;
}

