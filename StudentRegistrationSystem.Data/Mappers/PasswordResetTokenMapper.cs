using System;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Data.Mappers;

/// <summary>
/// Mapper for PasswordResetToken entity using Dapper
/// </summary>
public static class PasswordResetTokenMapper
{
    /// <summary>
    /// Maps database row to PasswordResetToken entity
    /// </summary>
    public static PasswordResetToken Map(dynamic row)
    {
        return new PasswordResetToken
        {
            Id = row.Id ?? row.TokenId,
            UserId = row.UserId,
            Token = row.Token,
            ExpiresAt = row.ExpiresAt,
            IsUsed = row.IsUsed,
            CreatedAt = row.CreatedAt
        };
    }
}

