using System.Text;
using Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

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
