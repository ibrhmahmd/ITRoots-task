using System.Threading.Tasks;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);

    Task<bool> SendVerificationEmailAsync(string to, string fullName, string verificationLink);

    Task<bool> SendPasswordResetEmailAsync(string to, string fullName, string resetLink);
    Task<bool> SendWelcomeEmailAsync(string to, string fullName);
}

