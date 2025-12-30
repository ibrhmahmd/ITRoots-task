namespace StudentRegistrationSystem.Data.Queries;

/// <summary>
/// SQL queries for PasswordResetToken entity operations
/// </summary>
public static class PasswordResetTokenQueries
{
    public const string GetById = @"
        SELECT TokenId AS Id, UserId, Token, ExpiresAt, IsUsed, CreatedAt
        FROM PasswordResetTokens
        WHERE TokenId = @Id";

    public const string GetByToken = @"
        SELECT TokenId AS Id, UserId, Token, ExpiresAt, IsUsed, CreatedAt
        FROM PasswordResetTokens
        WHERE Token = @Token";

    public const string GetActiveByUserId = @"
        SELECT TokenId AS Id, UserId, Token, ExpiresAt, IsUsed, CreatedAt
        FROM PasswordResetTokens
        WHERE UserId = @UserId AND IsUsed = 0 AND ExpiresAt > GETDATE()
        ORDER BY CreatedAt DESC";

    public const string Create = @"
        INSERT INTO PasswordResetTokens (TokenId, UserId, Token, ExpiresAt, 
                                        IsUsed, CreatedAt)
        VALUES (@Id, @UserId, @Token, @ExpiresAt, @IsUsed, @CreatedAt)";

    public const string Update = @"
        UPDATE PasswordResetTokens
        SET Token = @Token,
            ExpiresAt = @ExpiresAt,
            IsUsed = @IsUsed
        WHERE TokenId = @Id";

    public const string MarkAsUsed = @"
        UPDATE PasswordResetTokens
        SET IsUsed = 1
        WHERE TokenId = @Id";

    public const string DeleteExpired = @"
        DELETE FROM PasswordResetTokens
        WHERE ExpiresAt < GETDATE() AND IsUsed = 1";
}

