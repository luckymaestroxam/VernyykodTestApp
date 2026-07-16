using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.RequestHandlers.LogoutUser;
using AutoFixture;
using Domain.Aggregates;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace UnitTests.Application.RequestHandlers.LogoutUser;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class LogoutUserRequestHandlerTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _tokenService = Substitute.For<ITokenService>();
        _revokedTokenWriteRepository = Substitute.For<IRevokedTokenWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new LogoutUserRequestHandler(_tokenService, _revokedTokenWriteRepository, _unitOfWork);
    }

    [TearDown]
    public void TearDown()
    {
        _revokedTokenWriteRepository.ClearReceivedCalls();
        _unitOfWork.ClearReceivedCalls();
    }

    private static readonly Fixture Fixture = new();
    private ITokenService _tokenService;
    private IRevokedTokenWriteRepository _revokedTokenWriteRepository;
    private IUnitOfWork _unitOfWork;
    private LogoutUserRequestHandler _handler;

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Handle_WhenTokenIsMissing_ThrowsUnauthorizedAccessException(string token)
    {
        var request = new LogoutUserRequest(token);
        var ct = CancellationToken.None;

        Assert.Multiple(() =>
        {
            var exception =
                Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Токен отсутствует."));
        });
    }

    [Test]
    public void Handle_WhenTokenIsInvalid_ThrowsUnauthorizedAccessException()
    {
        var request = Fixture.Create<LogoutUserRequest>();
        var ct = CancellationToken.None;
        _tokenService.GetTokenInfo(request.Token).Returns(TokenInfo.CreateInvalid());

        Assert.Multiple(() =>
        {
            var exception =
                Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Токен невалидный."));
        });
    }

    [Test]
    public void Handle_WhenTokenAlreadyRevoked_ThrowsUserAlreadyLogoutException()
    {
        var request = Fixture.Create<LogoutUserRequest>();
        var ct = CancellationToken.None;
        var jti = Guid.NewGuid();
        var userName = Fixture.Create<string>();
        _tokenService.GetTokenInfo(request.Token).Returns(TokenInfo.CreateValid(userName, jti.ToString()));
        var conflictException = new RepositoryConflictException("conflict", new Exception());
        _unitOfWork.SaveChanges(ct).ThrowsAsync(conflictException);

        Assert.Multiple(() =>
        {
            var exception = Assert.ThrowsAsync<UserAlreadyLogoutException>(async () =>
                await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Выход уже был произведен ранее."));
            Assert.That(exception.InnerException, Is.SameAs(conflictException));
        });
    }

    [Test]
    public async Task Handle_WhenTokenIsValid_ReturnsLogoutUserResponse()
    {
        var request = Fixture.Create<LogoutUserRequest>();
        var ct = CancellationToken.None;
        var jti = Guid.NewGuid();
        var userName = Fixture.Create<string>();
        _tokenService.GetTokenInfo(request.Token).Returns(TokenInfo.CreateValid(userName, jti.ToString()));
        _unitOfWork.SaveChanges(ct).Returns(Task.CompletedTask);

        var response = await _handler.Handle(request, ct);

        Assert.That(response, Is.Not.Null);
        await _revokedTokenWriteRepository.Received(1).Add(Arg.Is<RevokedToken>(rt => rt.Jti == jti), ct);
        await _unitOfWork.Received(1).SaveChanges(ct);
    }
}
