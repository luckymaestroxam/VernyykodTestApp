using Microsoft.AspNetCore.Diagnostics;
using Shared.Application.Exceptions;
using UserService.Application.Exceptions;

namespace UserService.Api.Startup;

public static class StartupExceptionHandler
{
    public static void UseAppExceptionHandler(this WebApplication app) =>
        app.UseExceptionHandler(errApp =>
        {
            errApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                context.Response.StatusCode = exception switch
                {
                    DuplicateResourceException => StatusCodes.Status409Conflict,
                    UserAlreadyExistsException => StatusCodes.Status409Conflict,
                    UserAlreadyLogoutException => StatusCodes.Status409Conflict,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                var errorResponse = new { Error = exception?.Message };

                await context.Response.WriteAsJsonAsync(errorResponse);
            });
        });
}
