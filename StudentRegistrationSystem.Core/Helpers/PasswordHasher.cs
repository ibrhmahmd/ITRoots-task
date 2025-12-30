using BCrypt.Net;

namespace StudentRegistrationSystem.Core.Helpers;

/// <summary>
/// Helper class for password hashing and verification
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Hashes a password using BCrypt
    /// </summary>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

   
    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
