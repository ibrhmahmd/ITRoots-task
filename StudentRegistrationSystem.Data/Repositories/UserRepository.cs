using System;
using System.Threading.Tasks;
using Dapper;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Mappers;
using StudentRegistrationSystem.Data.Queries;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

/// <summary>
/// Repository implementation for User entity using Dapper
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        UserMapper.Configure();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetById,
            new { Id = id }
        );
        return result;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetByUsername,
            new { Username = username }
        );
        return result;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetByEmail,
            new { Email = email }
        );
        return result;
    }

    public async Task<User?> GetByEmailVerificationTokenAsync(string token)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetByEmailVerificationToken,
            new { Token = token }
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

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
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
            }
        );
        return user.Id;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
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
            }
        );
        return rowsAffected > 0;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            UserQueries.UsernameExists,
            new { Username = username }
        );
        return count > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            UserQueries.EmailExists,
            new { Email = email }
        );
        return count > 0;
    }
}
