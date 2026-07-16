using Application.Interfaces;
using Application.RequestHandlers.LoginUser;
using Application.RequestHandlers.LogoutUser;
using Application.RequestHandlers.RegisterUser;
using Infrastructure.Crypto;
using Infrastructure.Options;
using Infrastructure.Security;

namespace Api.Startup;

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

        var options = new JwtTokenServiceOptions(
            builder.Configuration.GetValue<string>("jwt:key")!,
            builder.Configuration.GetValue<string>("jwt:issuer")!,
            builder.Configuration.GetValue<int>("jwt:expiresTokenInHours"));
        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton(JwtTokenValidation.Create(options));
        builder.Services.AddSingleton<ITokenService, JwtTokenService>();
    }
}
