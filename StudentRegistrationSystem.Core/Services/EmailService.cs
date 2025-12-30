using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Core.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IConfiguration configuration)
    {
        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        _emailSettings = appSettings?.EmailSettings ?? new EmailSettings();
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(_emailSettings.SmtpUsername))
            {
                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
        catch
        {
            // Log error in production
            return false;
        }
    }

    public async Task<bool> SendVerificationEmailAsync(string to, string fullName, string verificationLink)
    {
        var subject = "Verify Your Email Address - Student Registration System";
        var body = EmailTemplateHelper.GetEmailVerificationBody(fullName, verificationLink);
        return await SendEmailAsync(to, subject, body);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string to, string fullName, string resetLink)
    {
        var subject = "Password Reset Request - Student Registration System";
        var body = EmailTemplateHelper.GetPasswordResetBody(fullName, resetLink);
        return await SendEmailAsync(to, subject, body);
    }

    public async Task<bool> SendWelcomeEmailAsync(string to, string fullName)
    {
        var subject = "Welcome to Student Registration System";
        var body = EmailTemplateHelper.GetWelcomeEmailBody(fullName);
        return await SendEmailAsync(to, subject, body);
    }
}
