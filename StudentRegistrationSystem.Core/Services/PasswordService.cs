using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class PasswordService : IPasswordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PasswordService> _logger;

    public PasswordService(IUnitOfWork unitOfWork, ILogger<PasswordService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string userId)
    {
        // Generate token
        var token = TokenGenerator.GenerateToken();

        // Create token entity
        var passwordResetToken = new PasswordResetToken
        {
            Id = TokenGenerator.GenerateToken(),
            UserId = userId,
            Token = token,
            ExpiresAt = DateTimeHelper.AddHours(1), // Token expires in 1 hour
            IsUsed = false,
            CreatedAt = DateTimeHelper.UtcNow
        };

        await _unitOfWork.PasswordResetTokens.CreateAsync(passwordResetToken);
        _logger.LogInformation("Password reset token generated for user {UserId}", userId);
        
        return token;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var tokenEntity = await _unitOfWork.PasswordResetTokens.GetByTokenAsync(token);
        
        if (tokenEntity == null)
        {
            return false;
        }

        if (tokenEntity.IsUsed)
        {
            return false;
        }

        if (DateTimeHelper.IsPast(tokenEntity.ExpiresAt))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        // Validate token
        var tokenEntity = await _unitOfWork.PasswordResetTokens.GetByTokenAsync(token);
        
        if (tokenEntity == null)
        {
            throw new NotFoundException("Invalid or expired token");
        }

        if (tokenEntity.IsUsed)
        {
            throw new BusinessException("Token has already been used");
        }

        if (DateTimeHelper.IsPast(tokenEntity.ExpiresAt))
        {
            throw new BusinessException("Token has expired");
        }

        // Get user
        var user = await _unitOfWork.Users.GetByIdAsync(tokenEntity.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        // Update password
        user.PasswordHash = PasswordHasher.HashPassword(newPassword);
        user.UpdatedAt = DateTimeHelper.UtcNow;

        var updated = await _unitOfWork.Users.UpdateAsync(user);
        
        if (updated)
        {
            // Mark token as used
            await _unitOfWork.PasswordResetTokens.MarkAsUsedAsync(tokenEntity.Id);
            _logger.LogInformation("Password reset successfully for user {UserId}", user.Id);
        }

        return updated;
    }
}
