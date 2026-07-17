using Application.Exceptions;
using Application.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;

namespace Application.RequestHandlers.RegisterUser;

public class RegisterUserRequestHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var userName = UserName.Create(request.Name);
        var password = PlainPassword.Create(request.Password);
        var passwordData = passwordHasher.Hash(password);
        var user = User.Create(userName, passwordData);

        try
        {
            await userWriteRepository.Add(user, cancellationToken);
            await unitOfWork.SaveChanges(cancellationToken);
        }
        catch (DuplicateResourceException ex)
        {
            throw new UserAlreadyExistsException("Пользователь уже существует.", ex);
        }

        var token = tokenService.GetToken(user.Id, user.Name.Value);
        return new RegisterUserResponse(user.Id, user.Name.Value, token);
    }
}
