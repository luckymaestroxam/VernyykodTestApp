using System.Security.Cryptography;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Crypto;

public class Pbkdf2PasswordHasher : IPasswordHasher, IPasswordVerifier
{
    private const string Scheme = "pbkdf2";
    private const string Algorithm = "sha256";
    private const int Iterations = 100_000;
    private const int SaltSize = 32;
    private const int HashSize = 32;
    private const char Separator = '$';

    public PasswordData Hash(PlainPassword plainPassword)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = GetPasswordHashBytes(plainPassword.Value, salt, Iterations);

        return PasswordData.Create(string.Join(Separator, Scheme, Algorithm, Iterations.ToString(),
            Convert.ToBase64String(salt), Convert.ToBase64String(hash)));
    }

    public bool Matches(PlainPassword plainPassword, PasswordData passwordData)
    {
        var parts = passwordData.Value.Split(Separator);
        if (!parts[0].Equals(Scheme, StringComparison.OrdinalIgnoreCase) ||
            !parts[1].Equals(Algorithm, StringComparison.OrdinalIgnoreCase) ||
            !int.TryParse(parts[2], out var iterations))
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(parts[3]);
            var expectedHash = Convert.FromBase64String(parts[4]);
            var actualHash = GetPasswordHashBytes(plainPassword.Value, salt, iterations);

            return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static byte[] GetPasswordHashBytes(string password, byte[] salt, int iterations) =>
        Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations,
            HashAlgorithmName.SHA256, HashSize);
}
