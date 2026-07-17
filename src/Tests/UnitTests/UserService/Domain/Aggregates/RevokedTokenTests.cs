using AutoFixture;
using NUnit.Framework;
using Shared.Domain.Aggregates;

namespace UnitTests.UserService.Domain.Aggregates;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class RevokedTokenTests
{
    private static readonly Fixture Fixture = new();

    [Test]
    public void Create_WhenArgumentsAreValid_ReturnsRevokedToken()
    {
        var jti = Fixture.Create<Guid>();
        var revokedAt = Fixture.Create<DateTime>();

        var result = RevokedToken.Create(jti, revokedAt);

        Assert.Multiple(() =>
        {
            Assert.That(result.Jti, Is.EqualTo(jti));
            Assert.That(result.RevokedAt, Is.EqualTo(revokedAt));
        });
    }
}
