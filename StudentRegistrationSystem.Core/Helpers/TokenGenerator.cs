using System;

namespace StudentRegistrationSystem.Core.Helpers;

/// <summary>
/// Helper class for generating tokens
/// </summary>
public static class TokenGenerator
{
    /// <summary>
    /// Generates a secure random token
    /// </summary>
    public static string GenerateToken()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Generates a token with a specific format
    /// </summary>
    public static string GenerateToken(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid()}";
    }

    /// <summary>
    /// Generates a base64 encoded token
    /// </summary>
    public static string GenerateBase64Token()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
