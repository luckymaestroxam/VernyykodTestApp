using AutoFixture;
using FinanceService.Domain.Aggregates;
using NUnit.Framework;
using Shared.Domain.ValueObjects;

namespace UnitTests.FinanceService.Domain.Aggregates;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class UserCurrencyTests
{
    private static readonly Fixture Fixture = new();

    [Test]
    public void Create_WhenArgumentsAreValid_ReturnsUserCurrency()
    {
        var userId = Fixture.Create<Guid>();
        var currencyId = CurrencyId.Create("R01235");

        var result = UserCurrency.Create(userId, currencyId);

        Assert.Multiple(() =>
        {
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.CurrencyId, Is.EqualTo(currencyId));
        });
    }
}
