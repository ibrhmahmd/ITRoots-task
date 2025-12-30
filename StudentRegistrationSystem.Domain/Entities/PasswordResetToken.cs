using System;

namespace StudentRegistrationSystem.Domain.Entities;

public class PasswordResetToken : BaseEntity
{
    public string UserId { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    // Navigation property (optional)
    public User? User { get; set; }
}

