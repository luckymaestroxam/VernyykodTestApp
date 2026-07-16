namespace Domain.Aggregates;

public sealed record RevokedToken
{
    private RevokedToken(Guid jti, DateTime revokedAt)
    {
        Jti = jti;
        RevokedAt = revokedAt;
    }

    public Guid Jti { get; }
    public DateTime RevokedAt { get; }

    public static RevokedToken Create(Guid jti, DateTime revokedAt) => new(jti, revokedAt);
}
