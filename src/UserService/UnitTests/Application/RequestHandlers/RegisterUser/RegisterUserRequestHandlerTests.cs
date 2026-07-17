using Application.Exceptions;
using Application.Interfaces;
using Application.RequestHandlers.RegisterUser;
using AutoFixture;
using Domain.Aggregates;
using Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace UnitTests.Application.RequestHandlers.RegisterUser;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class RegisterUserRequestHandlerTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _userWriteRepository = Substitute.For<IUserWriteRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _tokenService = Substitute.For<ITokenService>();
        _handler = new RegisterUserRequestHandler(_userWriteRepository, _unitOfWork, _passwordHasher, _tokenService);
    }

    private static readonly Fixture Fixture = new();
    private IUserWriteRepository _userWriteRepository;
    private IUnitOfWork _unitOfWork;
    private IPasswordHasher _passwordHasher;
    private ITokenService _tokenService;
    private RegisterUserRequestHandler _handler;

    [Test]
    public void Handle_WhenUserAlreadyExists_ThrowsUserAlreadyExistsException()
    {
        var request = Fixture.Build<RegisterUserRequest>()
            .With(r => r.Password, "Password1")
            .Create();
        var ct = CancellationToken.None;
        var passwordData = PasswordData.Create(Fixture.Create<string>());
        _passwordHasher.Hash(PlainPassword.Create(request.Password)).Returns(passwordData);
        var conflictException = new RepositoryConflictException("conflict", new Exception());
        _unitOfWork.SaveChanges(ct).ThrowsAsync(conflictException);

        Assert.Multiple(() =>
        {
            var exception = Assert.ThrowsAsync<UserAlreadyExistsException>(async () =>
                await _handler.Handle(request, ct));
            Assert.That(exception?.Message, Is.EqualTo("Пользователь уже существует."));
        });
    }

    [Test]
    public async Task Handle_WhenRegistrationSucceeds_ReturnsRegisterUserResponse()
    {
        var request = Fixture.Build<RegisterUserRequest>()
            .With(r => r.Password, "Password1")
            .Create();
        var ct = CancellationToken.None;
        var password = PlainPassword.Create(request.Password);
        var passwordData = PasswordData.Create(password.Value);
        _passwordHasher.Hash(password).Returns(passwordData);
        var token = Fixture.Create<string>();
        User user = null;
        _userWriteRepository.Add(Arg.Do<User>(u => user = u), ct).Returns(Task.CompletedTask);
        _tokenService.GetToken(Arg.Is<Guid>(id => id == user.Id), Arg.Is<string>(name => name == user.Name.Value))
            .Returns(token);
        _unitOfWork.SaveChanges(ct).Returns(Task.CompletedTask);

        var response = await _handler.Handle(request, ct);

        Assert.Multiple(() =>
        {
            Assert.That(response.UserId, Is.EqualTo(user.Id));
            Assert.That(response.UserName, Is.EqualTo(request.Name));
            Assert.That(response.Token, Is.EqualTo(token));
        });
    }
}
