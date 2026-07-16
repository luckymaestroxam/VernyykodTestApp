using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        builder.Services.AddAuthorization();
    }
}
