namespace StudentRegistrationSystem.Data.Queries;
public static class UserQueries
{
    public const string GetById = @"
        SELECT UserId AS Id, Username, PasswordHash, Email, Role, IsEmailVerified, 
               EmailVerificationToken, EmailVerificationTokenExpiry, IsActive, 
               CreatedAt, UpdatedAt
        FROM Users
        WHERE UserId = @Id";

    public const string GetByUsername = @"
        SELECT UserId AS Id, Username, PasswordHash, Email, Role, IsEmailVerified, 
               EmailVerificationToken, EmailVerificationTokenExpiry, IsActive, 
               CreatedAt, UpdatedAt
        FROM Users
        WHERE Username = @Username";

    public const string GetByEmail = @"
        SELECT UserId AS Id, Username, PasswordHash, Email, Role, IsEmailVerified, 
               EmailVerificationToken, EmailVerificationTokenExpiry, IsActive, 
               CreatedAt, UpdatedAt
        FROM Users
        WHERE Email = @Email";

    public const string GetByEmailVerificationToken = @"
        SELECT UserId AS Id, Username, PasswordHash, Email, Role, IsEmailVerified, 
               EmailVerificationToken, EmailVerificationTokenExpiry, IsActive, 
               CreatedAt, UpdatedAt
        FROM Users
        WHERE EmailVerificationToken = @Token";

    public const string Create = @"
        INSERT INTO Users (UserId, Username, PasswordHash, Email, Role, IsEmailVerified, 
                          EmailVerificationToken, EmailVerificationTokenExpiry, IsActive, 
                          CreatedAt, UpdatedAt)
        VALUES (@Id, @Username, @PasswordHash, @Email, @Role, @IsEmailVerified, 
                @EmailVerificationToken, @EmailVerificationTokenExpiry, @IsActive, 
                @CreatedAt, @UpdatedAt)";

    public const string Update = @"
        UPDATE Users
        SET Username = @Username,
            PasswordHash = @PasswordHash,
            Email = @Email,
            Role = @Role,
            IsEmailVerified = @IsEmailVerified,
            EmailVerificationToken = @EmailVerificationToken,
            EmailVerificationTokenExpiry = @EmailVerificationTokenExpiry,
            IsActive = @IsActive,
            UpdatedAt = @UpdatedAt
        WHERE UserId = @Id";

    public const string UsernameExists = @"
        SELECT COUNT(1)
        FROM Users
        WHERE Username = @Username";

    public const string EmailExists = @"
        SELECT COUNT(1)
        FROM Users
        WHERE Email = @Email";
}
