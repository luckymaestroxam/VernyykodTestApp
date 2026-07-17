namespace Infrastructure.Options;

public sealed record JwtTokenServiceOptions(string SymmetricSecurityKey, string Issuer, int ExpiresTokenInHours);
