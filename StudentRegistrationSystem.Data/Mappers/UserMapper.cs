using System;
using Dapper;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Data.Mappers;

/// <summary>
/// Mapper for User entity using Dapper
/// </summary>
public static class UserMapper
{
    /// <summary>
    /// Configures Dapper type mapping for User entity
    /// </summary>
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new UserRoleTypeHandler());
    }

    /// <summary>
    /// Maps database row to User entity
    /// </summary>
    public static User Map(dynamic row)
    {
        return new User
        {
            Id = row.Id ?? row.UserId,
            Username = row.Username,
            PasswordHash = row.PasswordHash,
            Email = row.Email,
            Role = (UserRole)Convert.ToInt32(row.Role),
            IsEmailVerified = row.IsEmailVerified,
            EmailVerificationToken = row.EmailVerificationToken,
            EmailVerificationTokenExpiry = row.EmailVerificationTokenExpiry,
            IsActive = row.IsActive,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt
        };
    }
}

public class UserRoleTypeHandler : Dapper.SqlMapper.TypeHandler<UserRole>
{
    public override void SetValue(System.Data.IDbDataParameter parameter, UserRole value)
    {
        parameter.Value = (int)value;
    }

    public override UserRole Parse(object value)
    {
        return (UserRole)Convert.ToInt32(value);
    }
}
