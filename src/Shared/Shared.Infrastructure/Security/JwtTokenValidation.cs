using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Options;

namespace Shared.Infrastructure.Security;

public static class JwtTokenValidation
{
    public static TokenValidationParameters Create(JwtTokenServiceOptions options) =>
        new()
        {
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SymmetricSecurityKey)),
            ValidateIssuerSigningKey = true
        };
}
