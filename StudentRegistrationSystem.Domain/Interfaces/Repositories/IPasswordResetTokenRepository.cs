using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByIdAsync(string id);

    Task<PasswordResetToken?> GetByTokenAsync(string token);

    Task<IEnumerable<PasswordResetToken>> GetActiveByUserIdAsync(string userId);

    Task<string> CreateAsync(PasswordResetToken token);

    Task<bool> UpdateAsync(PasswordResetToken token);

    Task<bool> MarkAsUsedAsync(string tokenId);

    Task<int> DeleteExpiredTokensAsync();
}

