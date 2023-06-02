using bcrypt = BCrypt.Net.BCrypt;

namespace AgriPen.Infrastructure.Helpers;

public static class PasswordHelper
{
    const string ALPHANUMERIC = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string Hash(string password)
    {
        return bcrypt.HashPassword(password);
    }

    public static bool Verify(string plainText, string hash)
    {
        return bcrypt.Verify(plainText, hash);
    }

    public static string GeneratePseudoRandomString(int length)
    {
        return Enumerable.Repeat(ALPHANUMERIC, length)
            .Select(s => s[Random.Shared.Next(s.Length)])
            .Aggregate("", (prev, current) => prev + current);
    }
}
