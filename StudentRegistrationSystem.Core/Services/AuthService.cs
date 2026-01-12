using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;
using StudentRegistrationSystem.Domain.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly string _fallbackBaseUrl;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        
        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        _fallbackBaseUrl = appSettings?.BaseUrl ?? "https://localhost:5001";
    }

    public async Task<UserDto> RegisterAsync(string fullName, string username, string password, string email, string? phone, int? academicYear)
    {
        // Check duplicates (Read operations, can be outside transaction or inside)
        if (await _unitOfWork.Users.UsernameExistsAsync(username))
        {
            throw new DuplicateException("Username already exists");
        }

        if (await _unitOfWork.Users.EmailExistsAsync(email))
        {
            throw new DuplicateException("Email already exists");
        }

        // Generate email verification token
        var verificationToken = TokenGenerator.GenerateToken();
        var tokenExpiry = DateTimeHelper.AddHours(24);

        // Create new user entity
        var user = new User
        {
            Id = TokenGenerator.GenerateToken(),
            Username = username,
            PasswordHash = PasswordHasher.HashPassword(password),
            Email = email,
            Role = UserRole.Student,
            IsEmailVerified = false,
            EmailVerificationToken = verificationToken,
            EmailVerificationTokenExpiry = tokenExpiry,
            IsActive = true,
            CreatedAt = DateTimeHelper.UtcNow
        };

        string userId;

        try
        {
            _unitOfWork.BeginTransaction();

            userId = await _unitOfWork.Users.CreateAsync(user);

            var student = new Student
            {
                Id = TokenGenerator.GenerateToken(),
                UserId = userId,
                FullName = fullName,
                Phone = phone,
                AcademicYear = academicYear.HasValue ? (AcademicYear)academicYear.Value : AcademicYear.FirstYear,
                EnrollmentDate = DateTimeHelper.UtcNow,
                CreatedAt = DateTimeHelper.UtcNow
            };

            await _unitOfWork.Students.CreateAsync(student);

            _unitOfWork.Commit();
            _logger.LogInformation("User {Username} registered successfully with ID {UserId}", username, userId);
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            _logger.LogError(ex, "Failed to register user {Username}", username);
            throw;
        }

        // Send verification email (after commit)
        try
        {
            var baseUrl = UrlHelper.GetBaseUrl(_httpContextAccessor.HttpContext, _fallbackBaseUrl);
            var verificationLink = $"{baseUrl.TrimEnd('/')}/Account/VerifyEmail?token={verificationToken}";
            await _emailService.SendVerificationEmailAsync(email, fullName, verificationLink);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send verification email to {Email}. User was created successfully.", email);
        }

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
        var user = await _unitOfWork.Users.GetByUsernameAsync(username);
        
        if (user == null || !user.IsActive)
        {
            return null;
        }

        if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        if (!user.IsEmailVerified)
        {
            throw new BusinessException("Please verify your email address before logging in. Check your inbox for the verification email.");
        }

        _logger.LogInformation("User {Username} logged in successfully", username);

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
        var user = await _unitOfWork.Users.GetByEmailVerificationTokenAsync(token);
        
        if (user == null || DateTimeHelper.IsPast(user.EmailVerificationTokenExpiry))
        {
            return false;
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpiry = null;
        user.UpdatedAt = DateTimeHelper.UtcNow;

        _logger.LogInformation("Email verified for user {UserId}", user.Id);

        return await _unitOfWork.Users.UpdateAsync(user);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        
        if (user == null)
        {
            return true;
        }

        var token = await _passwordService.GeneratePasswordResetTokenAsync(user.Id);
        
        var baseUrl = UrlHelper.GetBaseUrl(_httpContextAccessor.HttpContext, _fallbackBaseUrl);
        var resetLink = $"{baseUrl.TrimEnd('/')}/Account/ResetPassword?token={token}";
        
        return await _emailService.SendPasswordResetEmailAsync(user.Email, user.Username, resetLink);
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        return await _passwordService.ResetPasswordAsync(token, newPassword);
    }
}
