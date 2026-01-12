using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Mappers;
using StudentRegistrationSystem.Data.Queries;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

public class PasswordResetTokenRepository : BaseRepository, IPasswordResetTokenRepository
{
    public PasswordResetTokenRepository(IDbConnection connection, IDbTransaction? transaction) : base(connection, transaction)
    {
    }

    public async Task<PasswordResetToken?> GetByIdAsync(string id)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<PasswordResetToken>(
            PasswordResetTokenQueries.GetById,
            new { Id = id },
            transaction: _transaction
        );
        return result;
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<PasswordResetToken>(
            PasswordResetTokenQueries.GetByToken,
            new { Token = token },
            transaction: _transaction
        );
        return result;
    }

    public async Task<IEnumerable<PasswordResetToken>> GetActiveByUserIdAsync(string userId)
    {
        var results = await Connection.QueryAsync<PasswordResetToken>(
            PasswordResetTokenQueries.GetActiveByUserId,
            new { UserId = userId },
            transaction: _transaction
        );
        return results;
    }

    public async Task<string> CreateAsync(PasswordResetToken token)
    {
        // Generate GUID if not set
        if (string.IsNullOrEmpty(token.Id))
        {
            token.Id = Guid.NewGuid().ToString();
        }

        if (token.CreatedAt == default)
        {
            token.CreatedAt = DateTime.UtcNow;
        }

        await Connection.ExecuteAsync(
            PasswordResetTokenQueries.Create,
            new
            {
                Id = token.Id,
                token.UserId,
                token.Token,
                token.ExpiresAt,
                token.IsUsed,
                token.CreatedAt
            },
            transaction: _transaction
        );
        return token.Id;
    }

    public async Task<bool> UpdateAsync(PasswordResetToken token)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            PasswordResetTokenQueries.Update,
            new
            {
                Id = token.Id,
                token.Token,
                token.ExpiresAt,
                token.IsUsed
            },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<bool> MarkAsUsedAsync(string tokenId)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            PasswordResetTokenQueries.MarkAsUsed,
            new { Id = tokenId },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<int> DeleteExpiredTokensAsync()
    {
        var rowsAffected = await Connection.ExecuteAsync(PasswordResetTokenQueries.DeleteExpired, transaction: _transaction);
        return rowsAffected;
    }
}

