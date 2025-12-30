using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(string fullName, string username, string password, string email, string? phone, int? academicYear);
    Task<UserDto?> LoginAsync(string username, string password);
    Task<bool> VerifyEmailAsync(string token);
    Task<bool> SendPasswordResetEmailAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
}

