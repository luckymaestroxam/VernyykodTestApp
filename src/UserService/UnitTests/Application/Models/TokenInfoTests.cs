using Application.Models;
using AutoFixture;
using NUnit.Framework;

namespace UnitTests.Application.Models;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class TokenInfoTests
{
    private static readonly Fixture Fixture = new();

    [Test]
    public void CreateValid_WhenJtiIsValid_ReturnsTokenInfo()
    {
        var userName = Fixture.Create<string>();
        var jti = Guid.NewGuid();

        var result = TokenInfo.CreateValid(userName, jti.ToString());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.UserName, Is.EqualTo(userName));
            Assert.That(result.Jti, Is.EqualTo(jti));
        });
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("not-a-guid")]
    public void CreateValid_WhenJtiIsInvalid_ThrowsArgumentException(string jti) =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                TokenInfo.CreateValid(Fixture.Create<string>(), jti));
            Assert.That(exception?.Message, Is.EqualTo("Указан некорректный jti."));
        });

    [Test]
    public void CreateInvalid_ReturnsInvalidTokenInfo()
    {
        var result = TokenInfo.CreateInvalid();

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.UserName, Is.EqualTo(string.Empty));
            Assert.That(result.Jti, Is.EqualTo(Guid.Empty));
        });
    }
}
