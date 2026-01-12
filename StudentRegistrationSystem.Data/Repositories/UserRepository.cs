using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Mappers;
using StudentRegistrationSystem.Data.Queries;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IDbConnection connection, IDbTransaction? transaction) : base(connection, transaction)
    {
        UserMapper.Configure();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetById,
            new { Id = id },
            transaction: _transaction
        );
        return result;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetByUsername,
            new { Username = username },
            transaction: _transaction
        );
        return result;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetByEmail,
            new { Email = email },
            transaction: _transaction
        );
        return result;
    }

    public async Task<User?> GetByEmailVerificationTokenAsync(string token)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetByEmailVerificationToken,
            new { Token = token },
            transaction: _transaction
        );
        return result;
    }

    public async Task<string> CreateAsync(User user)
    {
        // Generate GUID if not set
        if (string.IsNullOrEmpty(user.Id))
        {
            user.Id = Guid.NewGuid().ToString();
        }

        if (user.CreatedAt == default)
        {
            user.CreatedAt = DateTime.UtcNow;
        }

        await Connection.ExecuteAsync(
            UserQueries.Create,
            new
            {
                Id = user.Id,
                user.Username,
                user.PasswordHash,
                user.Email,
                Role = (int)user.Role,
                user.IsEmailVerified,
                user.EmailVerificationToken,
                user.EmailVerificationTokenExpiry,
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt
            },
            transaction: _transaction
        );
        return user.Id;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            UserQueries.Update,
            new
            {
                Id = user.Id,
                user.Username,
                user.PasswordHash,
                user.Email,
                Role = (int)user.Role,
                user.IsEmailVerified,
                user.EmailVerificationToken,
                user.EmailVerificationTokenExpiry,
                user.IsActive,
                UpdatedAt = DateTime.UtcNow
            },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var count = await Connection.QuerySingleAsync<int>(
            UserQueries.UsernameExists,
            new { Username = username },
            transaction: _transaction
        );
        return count > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var count = await Connection.QuerySingleAsync<int>(
            UserQueries.EmailExists,
            new { Email = email },
            transaction: _transaction
        );
        return count > 0;
    }
}
