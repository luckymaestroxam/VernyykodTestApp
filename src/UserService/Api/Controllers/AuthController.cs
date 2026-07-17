using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using UserService.Application.RequestHandlers.LoginUser;
using UserService.Application.RequestHandlers.LogoutUser;
using UserService.Application.RequestHandlers.RegisterUser;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
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
        await logoutUserRequestHandler.Handle(new LogoutUserRequest(Jti), cancellationToken);

        return NoContent();
    }
}
