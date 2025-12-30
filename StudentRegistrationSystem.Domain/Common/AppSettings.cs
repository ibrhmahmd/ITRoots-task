using System.Collections.Generic;

namespace StudentRegistrationSystem.Domain.Common;

/// <summary>
/// Application settings configuration class
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Email verification token expiration in hours
    /// </summary>
    public int VerificationTokenExpirationHours { get; set; } = 24;

    /// <summary>
    /// Password reset token expiration in hours
    /// </summary>
    public int PasswordResetTokenExpirationHours { get; set; } = 1;

    /// <summary>
    /// Default language
    /// </summary>
    public string DefaultLanguage { get; set; } = "en";

    /// <summary>
    /// Supported languages
    /// </summary>
    public List<string> SupportedLanguages { get; set; } = new() { "en", "ar" };

    /// <summary>
    /// Email settings
    /// </summary>
    public EmailSettings EmailSettings { get; set; } = new();
}

/// <summary>
/// Email configuration settings
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// SMTP server address
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// SMTP server port
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// Sender email address
    /// </summary>
    public string SenderEmail { get; set; } = string.Empty;

    /// <summary>
    /// Sender name
    /// </summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// SMTP username
    /// </summary>
    public string SmtpUsername { get; set; } = string.Empty;

    /// <summary>
    /// SMTP password
    /// </summary>
    public string SmtpPassword { get; set; } = string.Empty;

    /// <summary>
    /// Enable SSL
    /// </summary>
    public bool EnableSsl { get; set; } = true;
}
