using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models;
using AutoFixture;
using Domain.Aggregates;
using Domain.ValueObjects;
using Infrastructure.Options;
using Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace UnitTests.Infrastructure.Security;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class TokenServiceTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var options = new JwtTokenServiceOptions(TestSigningKey, TestIssuer, 24);

        _service = new JwtTokenService(options, JwtTokenValidation.Create(options));
    }

    private static readonly Fixture Fixture = new();
    private const string TestIssuer = "test-issuer";
    private const string TestSigningKey = "test-VernyyKod_key_3dfe6cb4-3333-2222-3333-a4978df8444";
    private static readonly JwtSecurityTokenHandler JwtHandler = new();

    private JwtTokenService _service;

    [Test]
    public void GetToken_WhenUserPresent_ReturnsTokenWithExpectedClaims()
    {
        var user = User.Create(UserName.Create(Fixture.Create<string>()),
            PasswordData.Create(Fixture.Create<string>()));

        var result = _service.GetToken(user);
        var jwt = JwtHandler.ReadJwtToken(result);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(jwt.Issuer, Is.EqualTo(TestIssuer));
            Assert.That(jwt.Claims.First(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value,
                Is.EqualTo(user.Name.Value));
            Assert.That(jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value,
                Is.EqualTo(user.Id.ToString()));
            Assert.That(Guid.TryParse(jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value, out _),
                Is.True);
        });
    }

    [Test]
    public void GetTokenInfo_WhenTokenValid_ReturnsValidTokenInfo()
    {
        var userName = Fixture.Create<string>();
        var token = _service.GetToken(User.Create(UserName.Create(userName),
            PasswordData.Create(Fixture.Create<string>())));

        var result = _service.GetTokenInfo(token);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.UserName, Is.EqualTo(userName));
            Assert.That(result.Jti, Is.Not.EqualTo(Guid.Empty));
        });
    }

    [Test]
    public void GetTokenInfo_WhenTokenInvalid_ReturnsDefault()
    {
        var result = _service.GetTokenInfo(Fixture.Create<string>());

        Assert.That(result, Is.EqualTo(TokenInfo.CreateInvalid()));
    }

    [Test]
    public void GetTokenInfo_WhenTokenExpired_ReturnsDefault()
    {
        var expiredToken = CreateToken(
            TestIssuer,
            TestSigningKey,
            Fixture.Create<string>(),
            Guid.NewGuid().ToString(),
            DateTime.UtcNow.AddHours(-2),
            DateTime.UtcNow.AddHours(-1));

        var result = _service.GetTokenInfo(expiredToken);

        Assert.That(result, Is.EqualTo(TokenInfo.CreateInvalid()));
    }

    [Test]
    public void GetTokenInfo_WhenIssuerIsWrong_ReturnsDefault()
    {
        var token = CreateToken(
            "wrong-issuer",
            TestSigningKey,
            Fixture.Create<string>(),
            Guid.NewGuid().ToString(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1));

        var result = _service.GetTokenInfo(token);

        Assert.That(result, Is.EqualTo(TokenInfo.CreateInvalid()));
    }

    [Test]
    public void GetTokenInfo_WhenSignatureIsInvalid_ReturnsDefault()
    {
        var token = CreateToken(
            TestIssuer,
            "other-VernyyKod_key_aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
            Fixture.Create<string>(),
            Guid.NewGuid().ToString(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1));

        var result = _service.GetTokenInfo(token);

        Assert.That(result, Is.EqualTo(TokenInfo.CreateInvalid()));
    }

    [Test]
    public void GetTokenInfo_WhenNameClaimIsMissing_ReturnsDefault()
    {
        var token = CreateToken(
            TestIssuer,
            TestSigningKey,
            Fixture.Create<string>(),
            Guid.NewGuid().ToString(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            false);

        var result = _service.GetTokenInfo(token);

        Assert.That(result, Is.EqualTo(TokenInfo.CreateInvalid()));
    }

    private static string CreateToken(
        string issuer,
        string signingKey,
        string userName,
        string jti,
        DateTime notBefore,
        DateTime expires,
        bool includeNameClaim = true)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Jti, jti) };
        if (includeNameClaim)
        {
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, userName));
        }

        var jwt = new JwtSecurityToken(
            issuer,
            claims: claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: credentials);

        return JwtHandler.WriteToken(jwt);
    }
}
