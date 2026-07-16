namespace Application.RequestHandlers.LoginUser;

public sealed record LoginUserResponse(Guid Id, string Name, string Token);
