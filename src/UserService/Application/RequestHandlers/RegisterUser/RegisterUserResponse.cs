namespace Application.RequestHandlers.RegisterUser;

public sealed record RegisterUserResponse(Guid Id, string Name, string Token);
