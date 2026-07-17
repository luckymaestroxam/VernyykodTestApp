using Application.Interfaces;
using Application.Models;
using Application.RequestHandlers.LoginUser;
using AutoFixture;
using Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace UnitTests.Application.RequestHandlers.LoginUser;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class LoginUserRequestHandlerTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _userReadRepository = Substitute.For<IUserReadRepository>();
        _tokenService = Substitute.For<ITokenService>();
        _passwordVerifier = Substitute.For<IPasswordVerifier>();
        _handler = new LoginUserRequestHandler(_userReadRepository, _tokenService, _passwordVerifier);
    }

    private static readonly Fixture Fixture = new();
    private IUserReadRepository _userReadRepository;
    private ITokenService _tokenService;
    private IPasswordVerifier _passwordVerifier;
    private LoginUserRequestHandler _handler;

    [Test]
    public void Handle_WhenUserNotFound_ThrowsUnauthorizedAccessException()
    {
        var request = Fixture.Create<LoginUserRequest>();
        var ct = CancellationToken.None;
        _userReadRepository.Get(request.Name, ct).ReturnsNull();

        Assert.Multiple(() =>
        {
            var exception =
                Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Неверный логин или пароль."));
        });
    }

    [Test]
    public void Handle_WhenPasswordDoesNotMatch_ThrowsUnauthorizedAccessException()
    {
        var request = Fixture.Build<LoginUserRequest>()
            .With(r => r.Password, "Password1")
            .Create();
        var ct = CancellationToken.None;
        var user = new UserDto(Guid.NewGuid(), request.Name, request.Password);
        _userReadRepository.Get(request.Name, ct).Returns(user);
        _passwordVerifier.Matches(PlainPassword.Create(request.Password), Arg.Any<PasswordData>())
            .Returns(false);

        Assert.Multiple(() =>
        {
            var exception =
                Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Неверный логин или пароль."));
        });
    }

    [Test]
    public async Task Handle_WhenCredentialsAreValid_ReturnsLoginUserResponse()
    {
        var request = Fixture.Build<LoginUserRequest>()
            .With(r => r.Password, "Password1")
            .Create();
        var ct = CancellationToken.None;
        var user = new UserDto(Guid.NewGuid(), request.Name, request.Password);
        _userReadRepository.Get(request.Name, ct).Returns(user);
        _passwordVerifier.Matches(PlainPassword.Create(request.Password), Arg.Any<PasswordData>())
            .Returns(true);
        var token = Fixture.Create<string>();
        _tokenService.GetToken(user.Id, user.Name).Returns(token);

        var response = await _handler.Handle(request, ct);

        Assert.Multiple(() =>
        {
            Assert.That(response.UserId, Is.EqualTo(user.Id));
            Assert.That(response.UserName, Is.EqualTo(user.Name));
            Assert.That(response.Token, Is.EqualTo(token));
        });
    }
}
