using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Options;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Security;

public class JwtTokenService(JwtTokenServiceOptions options, TokenValidationParameters validationParameters)
    : ITokenService
{
    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    private readonly int _expiresTokenInHours = options.ExpiresTokenInHours;
    private readonly string _issuer = options.Issuer;

    private readonly SigningCredentials _signingCredentials = new(validationParameters.IssuerSigningKey,
        SecurityAlgorithms.HmacSha256);

    public string GetToken(Guid userId, string userName)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userName),
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwt = new JwtSecurityToken(
            _issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(_expiresTokenInHours),
            signingCredentials: _signingCredentials
        );

        return JwtSecurityTokenHandler.WriteToken(jwt);
    }
}
