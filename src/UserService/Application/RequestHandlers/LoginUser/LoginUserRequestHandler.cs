using Application.Interfaces;
using Domain.ValueObjects;

namespace Application.RequestHandlers.LoginUser;

public class LoginUserRequestHandler(
    IUserReadRepository userReadRepository,
    ITokenService tokenService,
    IPasswordVerifier passwordVerifier)
    : IRequestHandler<LoginUserRequest, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userReadRepository.Get(request.Name, cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Неверный логин или пароль.");
        }

        var password = PlainPassword.Create(request.Password);
        if (!passwordVerifier.Matches(password, PasswordData.Create(user.PasswordHash)))
        {
            throw new UnauthorizedAccessException("Неверный логин или пароль.");
        }

        var token = tokenService.GetToken(user.Id, user.Name);

        return new LoginUserResponse(user.Id, user.Name, token);
    }
}
