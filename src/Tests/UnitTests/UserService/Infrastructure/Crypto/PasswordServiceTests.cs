using NUnit.Framework;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Crypto;

namespace UnitTests.UserService.Infrastructure.Crypto;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class PasswordServiceTests
{
    private const string ValidPassword = "Password1";
    private readonly Pbkdf2PasswordHasher _passwordHasher = new();

    [Test]
    public void Hash_WhenCalled_ReturnsExpectedFormat()
    {
        var passwordData = _passwordHasher.Hash(PlainPassword.Create(ValidPassword));

        var parts = passwordData.Value.Split('$');
        Assert.Multiple(() =>
        {
            Assert.That(parts, Has.Length.EqualTo(5));
            Assert.That(parts[0], Is.EqualTo("pbkdf2").IgnoreCase);
            Assert.That(parts[1], Is.EqualTo("sha256").IgnoreCase);
            Assert.That(parts[2], Is.EqualTo("100000"));
            Assert.That(() => Convert.FromBase64String(parts[3]), Throws.Nothing);
            Assert.That(() => Convert.FromBase64String(parts[4]), Throws.Nothing);
        });
    }

    [Test]
    public void Hash_WhenCalledTwice_ReturnsDifferentHashes()
    {
        var plainPassword = PlainPassword.Create(ValidPassword);

        var first = _passwordHasher.Hash(plainPassword);
        var second = _passwordHasher.Hash(plainPassword);

        Assert.That(first.Value, Is.Not.EqualTo(second.Value));
    }

    [Test]
    public void Matches_WhenPasswordMatches_ReturnsTrue()
    {
        var plainPassword = PlainPassword.Create(ValidPassword);
        var passwordData = _passwordHasher.Hash(plainPassword);

        var result = _passwordHasher.Matches(plainPassword, passwordData);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Matches_WhenPasswordDoesNotMatch_ReturnsFalse()
    {
        var passwordData = _passwordHasher.Hash(PlainPassword.Create(ValidPassword));

        var result = _passwordHasher.Matches(PlainPassword.Create(ValidPassword + "x"), passwordData);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Matches_WhenPasswordDataHasInvalidFormat_ReturnsFalse()
    {
        var plainPassword = PlainPassword.Create(ValidPassword);
        var passwordData = PasswordData.Create("invalid-format");

        var result = _passwordHasher.Matches(plainPassword, passwordData);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Matches_WhenPasswordDataHasInvalidBase64_ReturnsFalse()
    {
        var plainPassword = PlainPassword.Create(ValidPassword);
        var passwordData = PasswordData.Create("pbkdf2$sha256$100000$not-base64$also-not-base64");

        var result = _passwordHasher.Matches(plainPassword, passwordData);

        Assert.That(result, Is.False);
    }
}
