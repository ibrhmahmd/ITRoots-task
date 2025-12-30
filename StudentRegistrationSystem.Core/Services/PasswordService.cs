using System;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Core.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public PasswordService(
        IPasswordResetTokenRepository tokenRepository,
        IUserRepository userRepository)
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string userId)
    {
        // Generate token
        var token = Guid.NewGuid().ToString();

        // Create token entity
        var passwordResetToken = new PasswordResetToken
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _tokenRepository.CreateAsync(passwordResetToken);
        return token;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var tokenEntity = await _tokenRepository.GetByTokenAsync(token);
        
        if (tokenEntity == null)
        {
            return false;
        }

        if (tokenEntity.IsUsed)
        {
            return false;
        }

        if (tokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        // Validate token
        var tokenEntity = await _tokenRepository.GetByTokenAsync(token);
        
        if (tokenEntity == null)
        {
            throw new NotFoundException("Invalid or expired token");
        }

        if (tokenEntity.IsUsed)
        {
            throw new BusinessException("Token has already been used");
        }

        if (tokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            throw new BusinessException("Token has expired");
        }

        // Get user
        var user = await _userRepository.GetByIdAsync(tokenEntity.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        // Update password
        user.PasswordHash = PasswordHasher.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;

        var updated = await _userRepository.UpdateAsync(user);
        
        if (updated)
        {
            // Mark token as used
            await _tokenRepository.MarkAsUsedAsync(tokenEntity.Id);
        }

        return updated;
    }
}
