using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for PasswordResetToken entity operations
/// </summary>
public interface IPasswordResetTokenRepository
{
    /// <summary>
    /// Gets a token by ID
    /// </summary>
    Task<PasswordResetToken?> GetByIdAsync(string id);

    /// <summary>
    /// Gets a token by token string
    /// </summary>
    Task<PasswordResetToken?> GetByTokenAsync(string token);

    /// <summary>
    /// Gets active tokens for a user
    /// </summary>
    Task<IEnumerable<PasswordResetToken>> GetActiveByUserIdAsync(string userId);

    /// <summary>
    /// Creates a new token
    /// </summary>
    Task<string> CreateAsync(PasswordResetToken token);

    /// <summary>
    /// Updates an existing token
    /// </summary>
    Task<bool> UpdateAsync(PasswordResetToken token);

    /// <summary>
    /// Marks a token as used
    /// </summary>
    Task<bool> MarkAsUsedAsync(string tokenId);

    /// <summary>
    /// Deletes expired tokens
    /// </summary>
    Task<int> DeleteExpiredTokensAsync();
}

