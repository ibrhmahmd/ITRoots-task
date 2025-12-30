using System;

namespace StudentRegistrationSystem.Domain.Entities;

/// <summary>
/// Represents a password reset token for a user
/// </summary>
public class PasswordResetToken : BaseEntity
{
    /// <summary>
    /// Foreign key to Users table
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Unique reset token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration date
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Indicates if the token has been used
    /// </summary>
    public bool IsUsed { get; set; }

    // Navigation property (optional)
    /// <summary>
    /// Related User entity
    /// </summary>
    public User? User { get; set; }
}

