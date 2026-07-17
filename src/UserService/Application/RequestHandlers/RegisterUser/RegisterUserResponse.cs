namespace UserService.Application.RequestHandlers.RegisterUser;

public sealed record RegisterUserResponse(Guid UserId, string UserName, string Token);
