using System;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Domain.Entities;


public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;


    public UserRole Role { get; set; }

    
    public bool IsEmailVerified { get; set; }

    public string? EmailVerificationToken { get; set; }

    
    public DateTime? EmailVerificationTokenExpiry { get; set; }

    public bool IsActive { get; set; }
}
