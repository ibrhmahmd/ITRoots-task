using System.Threading.Tasks;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface IPasswordService
{
    Task<string> GeneratePasswordResetTokenAsync(string userId);
    Task<bool> ValidateTokenAsync(string token);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
}

