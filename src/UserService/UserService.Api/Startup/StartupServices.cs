using Shared.Application.Interfaces;
using UserService.Application.Interfaces;
using UserService.Application.RequestHandlers.LoginUser;
using UserService.Application.RequestHandlers.LogoutUser;
using UserService.Application.RequestHandlers.RegisterUser;
using UserService.Infrastructure.Crypto;
using UserService.Infrastructure.Security;

namespace UserService.Api.Startup;

public static class StartupServices
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRequestHandler<RegisterUserRequest, RegisterUserResponse>,
            RegisterUserRequestHandler>();
        builder.Services.AddScoped<IRequestHandler<LoginUserRequest, LoginUserResponse>, LoginUserRequestHandler>();
        builder.Services.AddScoped<IRequestHandler<LogoutUserRequest, LogoutUserResponse>, LogoutUserRequestHandler>();
        builder.Services.AddSingleton<Pbkdf2PasswordHasher>();
        builder.Services.AddSingleton<IPasswordHasher>(sp => sp.GetRequiredService<Pbkdf2PasswordHasher>());
        builder.Services.AddSingleton<IPasswordVerifier>(sp => sp.GetRequiredService<Pbkdf2PasswordHasher>());
        builder.Services.AddSingleton<ITokenService, JwtTokenService>();
    }
}
