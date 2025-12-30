using System;
using Dapper;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Data.Mappers;

public static class UserMapper
{
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new UserRoleTypeHandler());
    }

    
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
