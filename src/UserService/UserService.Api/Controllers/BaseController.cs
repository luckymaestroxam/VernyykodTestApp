using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Api.Controllers;

public class BaseController : ControllerBase
{
    protected Guid Jti => GetJti();

    private Guid GetJti()
    {
        Guid.TryParse(User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value, out var userId);
        return userId;
    }
}
