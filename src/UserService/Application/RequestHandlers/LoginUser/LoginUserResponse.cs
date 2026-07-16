namespace Application.RequestHandlers.LoginUser;

public sealed record LoginUserResponse(Guid UserId, string UserName, string Token);
