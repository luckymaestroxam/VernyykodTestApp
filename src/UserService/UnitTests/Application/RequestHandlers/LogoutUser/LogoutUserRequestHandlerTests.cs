using Application.Exceptions;
using Application.Interfaces;
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
        _revokedTokenWriteRepository = Substitute.For<IRevokedTokenWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new LogoutUserRequestHandler(_revokedTokenWriteRepository, _unitOfWork);
    }

    [TearDown]
    public void TearDown()
    {
        _revokedTokenWriteRepository.ClearReceivedCalls();
        _unitOfWork.ClearReceivedCalls();
    }

    private static readonly Fixture Fixture = new();
    private IRevokedTokenWriteRepository _revokedTokenWriteRepository;
    private IUnitOfWork _unitOfWork;
    private LogoutUserRequestHandler _handler;

    [Test]
    public void Handle_WhenTokenAlreadyRevoked_ThrowsUserAlreadyLogoutException()
    {
        var request = Fixture.Create<LogoutUserRequest>();
        var ct = CancellationToken.None;
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
        _unitOfWork.SaveChanges(ct).Returns(Task.CompletedTask);

        var response = await _handler.Handle(request, ct);

        Assert.That(response, Is.Not.Null);
        await _revokedTokenWriteRepository.Received(1).Add(Arg.Is<RevokedToken>(rt => rt.Jti == request.Jti), ct);
        await _unitOfWork.Received(1).SaveChanges(ct);
    }
}
