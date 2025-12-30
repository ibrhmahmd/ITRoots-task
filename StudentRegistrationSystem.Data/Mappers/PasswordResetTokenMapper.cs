using System;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Data.Mappers;

public static class PasswordResetTokenMapper
{
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

