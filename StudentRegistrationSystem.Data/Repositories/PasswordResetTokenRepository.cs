using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Mappers;
using StudentRegistrationSystem.Data.Queries;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

/// <summary>
/// Repository implementation for PasswordResetToken entity using Dapper
/// </summary>
public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PasswordResetTokenRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PasswordResetToken?> GetByIdAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<PasswordResetToken>(
            PasswordResetTokenQueries.GetById,
            new { Id = id }
        );
        return result;
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<PasswordResetToken>(
            PasswordResetTokenQueries.GetByToken,
            new { Token = token }
        );
        return result;
    }

    public async Task<IEnumerable<PasswordResetToken>> GetActiveByUserIdAsync(string userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<PasswordResetToken>(
            PasswordResetTokenQueries.GetActiveByUserId,
            new { UserId = userId }
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

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            PasswordResetTokenQueries.Create,
            new
            {
                Id = token.Id,
                token.UserId,
                token.Token,
                token.ExpiresAt,
                token.IsUsed,
                token.CreatedAt
            }
        );
        return token.Id;
    }

    public async Task<bool> UpdateAsync(PasswordResetToken token)
    {
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            PasswordResetTokenQueries.Update,
            new
            {
                Id = token.Id,
                token.Token,
                token.ExpiresAt,
                token.IsUsed
            }
        );
        return rowsAffected > 0;
    }

    public async Task<bool> MarkAsUsedAsync(string tokenId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            PasswordResetTokenQueries.MarkAsUsed,
            new { Id = tokenId }
        );
        return rowsAffected > 0;
    }

    public async Task<int> DeleteExpiredTokensAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(PasswordResetTokenQueries.DeleteExpired);
        return rowsAffected;
    }
}

