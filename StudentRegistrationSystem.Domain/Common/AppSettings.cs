using System.Collections.Generic;

namespace StudentRegistrationSystem.Domain.Common;


public class AppSettings
{
    public int VerificationTokenExpirationHours { get; set; } = 24;

    
    public int PasswordResetTokenExpirationHours { get; set; } = 1;

    public string DefaultLanguage { get; set; } = "en";

    public List<string> SupportedLanguages { get; set; } = new() { "en", "ar" };

    public string BaseUrl { get; set; } = "https://localhost:5001";

    public EmailSettings EmailSettings { get; set; } = new();
}

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;

    public int SmtpPort { get; set; } = 587;

    public string SenderEmail { get; set; } = string.Empty;

    public string SenderName { get; set; } = string.Empty;

    public string SmtpUsername { get; set; } = string.Empty;

    public string SmtpPassword { get; set; } = string.Empty;

    public bool EnableSsl { get; set; } = true;
}
