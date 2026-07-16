using Application.Interfaces;
using Application.RequestHandlers.LoginUser;
using Application.RequestHandlers.LogoutUser;
using Application.RequestHandlers.RegisterUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request,
        [FromServices] IRequestHandler<RegisterUserRequest, RegisterUserResponse> registerUserRequestHandler,
        CancellationToken cancellationToken)
    {
        var result = await registerUserRequestHandler.Handle(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request,
        [FromServices] IRequestHandler<LoginUserRequest, LoginUserResponse> loginUserRequestHandler,
        CancellationToken cancellationToken)
    {
        var result = await loginUserRequestHandler.Handle(request, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromServices] IRequestHandler<LogoutUserRequest, LogoutUserResponse> logoutUserRequestHandler,
        CancellationToken cancellationToken)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        await logoutUserRequestHandler.Handle(new LogoutUserRequest(token ?? string.Empty), cancellationToken);

        return NoContent();
    }
}
