using AutoFixture;
using Domain.ValueObjects;
using NUnit.Framework;

namespace UnitTests.Domain.ValueObjects;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class PasswordDataTests
{
    private static readonly Fixture Fixture = new();

    [Test]
    public void Create_WhenValueIsValid_ReturnsPasswordData()
    {
        var value = Fixture.Create<string>();

        var result = PasswordData.Create(value);

        Assert.That(result.Value, Is.EqualTo(value));
    }

    [Test]
    public void Create_WhenValueIsNull_ThrowsArgumentNullException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PasswordData.Create(null!));
            Assert.That(exception?.ParamName, Is.EqualTo("Хеш пароля не может быть пустым."));
        });

    [TestCase("")]
    [TestCase("   ")]
    public void Create_WhenValueIsMissing_ThrowsArgumentException(string value) =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PasswordData.Create(value));
            Assert.That(exception?.ParamName, Is.EqualTo("Хеш пароля не может быть пустым."));
        });
}
