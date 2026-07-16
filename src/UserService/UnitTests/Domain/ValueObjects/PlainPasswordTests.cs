using Domain.ValueObjects;
using NUnit.Framework;

namespace UnitTests.Domain.ValueObjects;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class PlainPasswordTests
{
    [Test]
    public void Create_WhenValueIsValid_ReturnsPlainPassword()
    {
        const string value = "Password1";

        var result = PlainPassword.Create(value);

        Assert.That(result.Value, Is.EqualTo(value));
    }

    [TestCase("Passwor1")]
    [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1")]
    public void Create_WhenValueHasBoundaryLength_ReturnsPlainPassword(string value)
    {
        var result = PlainPassword.Create(value);

        Assert.That(result.Value, Is.EqualTo(value));
    }

    [Test]
    public void Create_WhenValueIsNull_ThrowsArgumentNullException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PlainPassword.Create(null!));
            Assert.That(exception?.ParamName, Is.EqualTo("Пароль не может быть пустым."));
        });

    [TestCase("")]
    [TestCase("   ")]
    public void Create_WhenValueIsMissing_ThrowsArgumentException(string value) =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PlainPassword.Create(value));
            Assert.That(exception?.ParamName, Is.EqualTo("Пароль не может быть пустым."));
        });

    [Test]
    public void Create_WhenValueIsTooShort_ThrowsArgumentException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PlainPassword.Create("Ab1"));
            Assert.That(exception?.Message, Is.EqualTo("Пароль должен содержать минимум 8 символов."));
        });

    [Test]
    public void Create_WhenValueIsTooLong_ThrowsArgumentException()
    {
        var value = new string('a', 64) + "1";

        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PlainPassword.Create(value));
            Assert.That(exception?.Message, Is.EqualTo("Пароль должен содержать максимум 64 символа."));
        });
    }

    [Test]
    public void Create_WhenValueContainsWhitespace_ThrowsArgumentException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PlainPassword.Create("Pass word1"));
            Assert.That(exception?.Message, Is.EqualTo("Пароль не должен содержать пробелы."));
        });

    [Test]
    public void Create_WhenValueHasNoLetter_ThrowsArgumentException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PlainPassword.Create("12345678"));
            Assert.That(exception?.Message, Is.EqualTo("Пароль должен содержать хотя бы одну букву."));
        });

    [Test]
    public void Create_WhenValueHasNoDigit_ThrowsArgumentException() =>
        Assert.Multiple(() =>
        {
            var exception = Assert.Throws<ArgumentException>(() => PlainPassword.Create("Password"));
            Assert.That(exception?.Message, Is.EqualTo("Пароль должен содержать хотя бы одну цифру."));
        });
}
