using AutoFixture;
using Domain.Aggregates;
using Domain.ValueObjects;
using NUnit.Framework;

namespace UnitTests.Domain.Aggregates;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class UserTests
{
    private static readonly Fixture Fixture = new();

    [Test]
    public void Create_WhenArgumentsAreValid_ReturnsUser()
    {
        var name = UserName.Create(Fixture.Create<string>());
        var passwordData = PasswordData.Create(Fixture.Create<string>());

        var result = User.Create(name, passwordData);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.PasswordData, Is.EqualTo(passwordData));
        });
    }

    [Test]
    public void Create_WhenNameIsNull_ThrowsArgumentNullException()
    {
        var passwordData = PasswordData.Create(Fixture.Create<string>());

        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentNullException>(() => User.Create(null!, passwordData));
            Assert.That(exception?.ParamName, Is.EqualTo("UserName не может быть пустым."));
        });
    }

    [Test]
    public void Create_WhenPasswordDataIsNull_ThrowsArgumentNullException()
    {
        var name = UserName.Create(Fixture.Create<string>());

        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentNullException>(() => User.Create(name, null!));
            Assert.That(exception?.ParamName, Is.EqualTo("PasswordData не может быть пустым."));
        });
    }
}
