using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _fallbackBaseUrl;

    public AuthService(
        IUserRepository userRepository,
        IStudentRepository studentRepository,
        IPasswordService passwordService,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _studentRepository = studentRepository;
        _passwordService = passwordService;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        
        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        _fallbackBaseUrl = appSettings?.BaseUrl ?? "https://localhost:5001";
    }

    public async Task<UserDto> RegisterAsync(string fullName, string username, string password, string email, string? phone, int? academicYear)
    {
        if (await _userRepository.UsernameExistsAsync(username))
        {
            throw new DuplicateException("Username already exists");
        }

        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new DuplicateException("Email already exists");
        }

        // Generate email verification token
        var verificationToken = Guid.NewGuid().ToString();
        var tokenExpiry = DateTime.UtcNow.AddHours(24); // Token expires in 24 hours

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = username,
            PasswordHash = PasswordHasher.HashPassword(password),
            Email = email,
            Role = UserRole.Student,
            IsEmailVerified = false,
            EmailVerificationToken = verificationToken,
            EmailVerificationTokenExpiry = tokenExpiry,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var userId = await _userRepository.CreateAsync(user);

        var student = new Student
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            FullName = fullName,
            Phone = phone,
            AcademicYear = academicYear.HasValue ? (AcademicYear)academicYear.Value : AcademicYear.FirstYear,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _studentRepository.CreateAsync(student);

        // Send verification email
        var baseUrl = GetBaseUrl();
        var verificationLink = $"{baseUrl.TrimEnd('/')}/Account/VerifyEmail?token={verificationToken}";
        await _emailService.SendVerificationEmailAsync(email, fullName, verificationLink);

        return new UserDto
        {
            Id = userId,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = user.IsEmailVerified,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null || !user.IsActive)
        {
            return null;
        }

        if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        // Check if email is verified
        if (!user.IsEmailVerified)
        {
            throw new BusinessException("Please verify your email address before logging in. Check your inbox for the verification email.");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = user.IsEmailVerified,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var user = await _userRepository.GetByEmailVerificationTokenAsync(token);
        
        if (user == null || user.EmailVerificationTokenExpiry < DateTime.UtcNow)
        {
            return false;
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        return await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null)
        {
            // Don't reveal if email exists
            return true;
        }

        var token = await _passwordService.GeneratePasswordResetTokenAsync(user.Id);
        
        var baseUrl = GetBaseUrl();
        var resetLink = $"{baseUrl.TrimEnd('/')}/Account/ResetPassword?token={token}";
        
        return await _emailService.SendPasswordResetEmailAsync(user.Email, user.Username, resetLink);
    }

    private string GetBaseUrl()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var request = httpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host;
            return $"{scheme}://{host}";
        }
        
        return _fallbackBaseUrl;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        return await _passwordService.ResetPasswordAsync(token, newPassword);
    }
}
