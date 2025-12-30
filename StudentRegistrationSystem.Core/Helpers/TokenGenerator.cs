using System;

namespace StudentRegistrationSystem.Core.Helpers;

public static class TokenGenerator
{
    public static string GenerateToken()
    {
        return Guid.NewGuid().ToString();
    }

    public static string GenerateToken(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid()}";
    }

    public static string GenerateBase64Token()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
