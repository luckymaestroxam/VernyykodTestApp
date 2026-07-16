using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.Models;
using Domain.Aggregates;
using Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    private readonly int _expiresTokenInHours;
    private readonly string _issuer;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenService(JwtTokenServiceOptions options)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SymmetricSecurityKey));
        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = securityKey,
            ValidateIssuerSigningKey = true
        };
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        _issuer = options.Issuer;
        _expiresTokenInHours = options.ExpiresTokenInHours;
    }

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

        var result = JwtSecurityTokenHandler.WriteToken(jwt);

        return result;
    }

    public TokenInfo GetTokenInfo(string token)
    {
        try
        {
            var principal = JwtSecurityTokenHandler.ValidateToken(token, _validationParameters, out _);
            var userName = principal.Identity?.Name;
            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return TokenInfo.CreateInvalid();
            }

            return TokenInfo.CreateValid(userName, jti);
        }
        catch (Exception ex) when (ex is SecurityTokenException or ArgumentException)
        {
            return TokenInfo.CreateInvalid();
        }
    }
}
