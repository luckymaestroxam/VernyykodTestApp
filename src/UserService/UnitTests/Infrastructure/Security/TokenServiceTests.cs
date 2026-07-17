using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoFixture;
using Domain.Aggregates;
using Domain.ValueObjects;
using Infrastructure.Options;
using Infrastructure.Security;
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

        var result = _service.GetToken(user.Id, user.Name.Value);
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
}
