using AutoFixture;
using FinanceService.Application.Exceptions;
using FinanceService.Application.Interfaces;
using FinanceService.Application.RequestHandlers.AddUserCurrency;
using FinanceService.Domain.Aggregates;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces;
using Shared.Domain.ValueObjects;

namespace UnitTests.FinanceService.Application.RequestHandlers.AddUserCurrency;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class AddUserCurrencyRequestHandlerTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _currencyReadRepository = Substitute.For<ICurrencyReadRepository>();
        _userCurrencyWriteRepository = Substitute.For<IUserCurrencyWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new AddUserCurrencyRequestHandler(_currencyReadRepository, _userCurrencyWriteRepository,
            _unitOfWork);
    }

    [TearDown]
    public void TearDown() => _unitOfWork.ClearReceivedCalls();

    private static readonly Fixture Fixture = new();
    private ICurrencyReadRepository _currencyReadRepository;
    private IUserCurrencyWriteRepository _userCurrencyWriteRepository;
    private IUnitOfWork _unitOfWork;
    private AddUserCurrencyRequestHandler _handler;

    [Test]
    public void Handle_WhenCurrencyDoesNotExist_ThrowsCurrencyNotExistsException()
    {
        var request = Fixture.Build<AddUserCurrencyRequest>()
            .With(a => a.CurrencyId, "R01530")
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
    public void Handle_WhenUserCurrencyAlreadyAdded_ThrowsUserCurrencyAlreadyAddedException()
    {
        var request = Fixture.Build<AddUserCurrencyRequest>()
            .With(a => a.CurrencyId, "R01530")
            .Create();
        var ct = CancellationToken.None;
        _currencyReadRepository.Exists(CurrencyId.Create(request.CurrencyId), ct).Returns(true);
        var conflictException = new DuplicateResourceException("conflict", new Exception());
        _unitOfWork.SaveChanges(ct).ThrowsAsync(conflictException);

        Assert.Multiple(() =>
        {
            var exception = Assert.ThrowsAsync<UserCurrencyAlreadyAddedException>(async () =>
                await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Избранная валюта уже добавлена."));
            Assert.That(exception.InnerException, Is.SameAs(conflictException));
        });
    }

    [Test]
    public async Task Handle_WhenUserCurrencyIsValid_ReturnsAddUserCurrencyResponse()
    {
        var request = Fixture.Build<AddUserCurrencyRequest>()
            .With(a => a.CurrencyId, "R01530")
            .Create();
        var ct = CancellationToken.None;
        _currencyReadRepository.Exists(CurrencyId.Create(request.CurrencyId), ct).Returns(true);
        _unitOfWork.SaveChanges(ct).Returns(Task.CompletedTask);

        var response = await _handler.Handle(request, ct);

        Assert.That(response, Is.Not.Null);
        await _userCurrencyWriteRepository.Received(1).Add(
            Arg.Is<UserCurrency>(uc => uc.UserId == request.UserId && uc.CurrencyId.Value == request.CurrencyId), ct);
        await _unitOfWork.Received(1).SaveChanges(ct);
    }
}
