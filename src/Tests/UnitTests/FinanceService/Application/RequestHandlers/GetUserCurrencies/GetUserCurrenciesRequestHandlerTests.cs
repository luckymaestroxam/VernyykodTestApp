using AutoFixture;
using FinanceService.Application.Interfaces;
using FinanceService.Application.Models;
using FinanceService.Application.RequestHandlers.GetUserCurrencies;
using NSubstitute;
using NUnit.Framework;

namespace UnitTests.FinanceService.Application.RequestHandlers.GetUserCurrencies;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class GetUserCurrenciesRequestHandlerTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _userCurrencyReadRepository = Substitute.For<IUserCurrencyReadRepository>();
        _handler = new GetUserCurrenciesRequestHandler(_userCurrencyReadRepository);
    }

    private static readonly Fixture Fixture = new();
    private IUserCurrencyReadRepository _userCurrencyReadRepository;
    private GetUserCurrenciesRequestHandler _handler;

    [Test]
    public async Task Handle_WhenUserCurrenciesExist_ReturnsGetUserCurrenciesResponse()
    {
        var request = Fixture.Create<GetUserCurrenciesRequest>();
        var ct = CancellationToken.None;
        var userCurrencies = Fixture.CreateMany<UserCurrencyDto>().ToArray();
        _userCurrencyReadRepository.GetMany(request.UserId, ct).Returns(userCurrencies);

        var response = await _handler.Handle(request, ct);

        Assert.That(response.UserCurrencies, Is.SameAs(userCurrencies));
    }
}
