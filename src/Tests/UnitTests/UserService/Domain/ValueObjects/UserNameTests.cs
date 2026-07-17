using AutoFixture;
using NUnit.Framework;
using UserService.Domain.ValueObjects;

namespace UnitTests.UserService.Domain.ValueObjects;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class UserNameTests
{
    private static readonly Fixture Fixture = new();

    [Test]
    public void Create_WhenValueIsValid_ReturnsUserName()
    {
        var value = Fixture.Create<string>();

        var result = UserName.Create(value);

        Assert.That(result.Value, Is.EqualTo(value));
    }

    [Test]
    public void Create_WhenValueIsNull_ThrowsArgumentNullException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentNullException>(() => UserName.Create(null!));
            Assert.That(exception?.ParamName, Is.EqualTo("Имя пользователя не может быть пустым."));
        });

    [TestCase("")]
    [TestCase("   ")]
    public void Create_WhenValueIsMissing_ThrowsArgumentException(string value) =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => UserName.Create(value));
            Assert.That(exception?.ParamName, Is.EqualTo("Имя пользователя не может быть пустым."));
        });
}
