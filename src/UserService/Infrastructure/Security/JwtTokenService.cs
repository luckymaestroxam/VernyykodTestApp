using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtTokenService(JwtTokenServiceOptions options, TokenValidationParameters validationParameters)
    : ITokenService
{
    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    private readonly int _expiresTokenInHours = options.ExpiresTokenInHours;
    private readonly string _issuer = options.Issuer;

    private readonly SigningCredentials _signingCredentials = new(validationParameters.IssuerSigningKey,
        SecurityAlgorithms.HmacSha256);

    public string GetToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Name.Value),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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
