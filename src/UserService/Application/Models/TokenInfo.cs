namespace Application.Models;

public sealed record TokenInfo
{
    private TokenInfo(bool isValid, string userName, string? jti)
    {
        IsValid = isValid;
        UserName = userName;
        if (!isValid)
        {
            return;
        }

        if (!Guid.TryParse(jti, out var jtiGuid))
        {
            throw new ArgumentException("Указан некорректный jti.");
        }

        Jti = jtiGuid;
    }

    public bool IsValid { get; }
    public string UserName { get; }
    public Guid Jti { get; }

    public static TokenInfo CreateValid(string userName, string? jti) => new(true, userName, jti);

    public static TokenInfo CreateInvalid() => new(false, string.Empty, string.Empty);
}
