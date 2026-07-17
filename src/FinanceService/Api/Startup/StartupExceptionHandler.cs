using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Startup;

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
                    CurrencyNotExistsException => StatusCodes.Status404NotFound,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                var errorResponse = new { Error = exception?.Message };

                await context.Response.WriteAsJsonAsync(errorResponse);
            });
        });
}
