using AutoFixture;
using FinanceService.Application.Exceptions;
using FinanceService.Application.Interfaces;
using FinanceService.Application.RequestHandlers.RemoveUserCurrency;
using FinanceService.Domain.Aggregates;
using NSubstitute;
using NUnit.Framework;
using Shared.Application.Interfaces;
using Shared.Domain.ValueObjects;

namespace UnitTests.FinanceService.Application.RequestHandlers.RemoveUserCurrency;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class RemoveUserCurrencyRequestHandlerTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _currencyReadRepository = Substitute.For<ICurrencyReadRepository>();
        _userCurrencyWriteRepository = Substitute.For<IUserCurrencyWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler =
            new RemoveUserCurrencyRequestHandler(_currencyReadRepository, _userCurrencyWriteRepository, _unitOfWork);
    }

    private static readonly Fixture Fixture = new();
    private ICurrencyReadRepository _currencyReadRepository;
    private IUserCurrencyWriteRepository _userCurrencyWriteRepository;
    private IUnitOfWork _unitOfWork;
    private RemoveUserCurrencyRequestHandler _handler;

    [Test]
    public void Handle_WhenCurrencyDoesNotExist_ThrowsCurrencyNotExistsException()
    {
        var request = Fixture.Build<RemoveUserCurrencyRequest>()
            .With(r => r.CurrencyId, "R01350")
            .Create();
        var ct = CancellationToken.None;
        _currencyReadRepository.Exists(CurrencyId.Create(request.CurrencyId), ct).Returns(false);

        Assert.Multiple(() =>
        {
            var exception = Assert.ThrowsAsync<CurrencyNotExistsException>(async () =>
                await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Валюта не найдена."));
        });
    }

    [Test]
    public async Task Handle_WhenUserCurrencyExists_ReturnsRemoveUserCurrencyResponse()
    {
        var request = Fixture.Build<RemoveUserCurrencyRequest>()
            .With(r => r.CurrencyId, "R01350")
            .Create();
        var ct = CancellationToken.None;
        _currencyReadRepository.Exists(CurrencyId.Create(request.CurrencyId), ct).Returns(true);
        _unitOfWork.SaveChanges(ct).Returns(Task.CompletedTask);

        var response = await _handler.Handle(request, ct);

        Assert.That(response, Is.Not.Null);
        await _userCurrencyWriteRepository.Received(1).Remove(
            Arg.Is<UserCurrency>(uc => uc.UserId == request.UserId && uc.CurrencyId.Value == request.CurrencyId), ct);
        await _unitOfWork.Received(1).SaveChanges(ct);
    }
}
