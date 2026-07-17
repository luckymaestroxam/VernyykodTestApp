using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces;
using Infrastructure.Options;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.Startup;

public static class StartupAuth
{
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<TokenValidationParameters>((options, validationParameters) =>
            {
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        if (!Guid.TryParse(context.Principal?.Claims
                                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value, out var jti))
                        {
                            context.Fail("В токене отсутствует корректный jti.");
                            return;
                        }

                        var revokedTokenReadRepository =
                            context.HttpContext.RequestServices.GetRequiredService<IRevokedTokenReadRepository>();
                        if (await revokedTokenReadRepository.Exists(jti, context.HttpContext.RequestAborted))
                        {
                            context.Fail("Токен отозван.");
                        }
                    }
                };
            });

        var options = new JwtTokenServiceOptions(
            builder.Configuration.GetValue<string>("jwt:key")!,
            builder.Configuration.GetValue<string>("jwt:issuer")!,
            builder.Configuration.GetValue<int>("jwt:expiresTokenInHours"));
        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton(JwtTokenValidation.Create(options));

        builder.Services.AddAuthorization();
    }
}
