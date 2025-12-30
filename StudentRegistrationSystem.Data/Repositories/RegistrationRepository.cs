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

public class RegistrationRepository : BaseRepository, IRegistrationRepository
{
    public RegistrationRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Registration?> GetByIdAsync(string id)
    {
        using var connection = CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Registration>(
            RegistrationQueries.GetById,
            new { Id = id }
        );
        return result;
    }

    public async Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId)
    {
        using var connection = CreateConnection();
        var results = await connection.QueryAsync<Registration>(
            RegistrationQueries.GetByStudentId,
            new { StudentId = studentId }
        );
        return results;
    }

    public async Task<IEnumerable<Registration>> GetActiveByStudentIdAsync(string studentId)
    {
        using var connection = CreateConnection();
        var results = await connection.QueryAsync<Registration>(
            RegistrationQueries.GetActiveByStudentId,
            new { StudentId = studentId }
        );
        return results;
    }

    public async Task<IEnumerable<Registration>> GetByCourseIdAsync(string courseId)
    {
        using var connection = CreateConnection();
        var results = await connection.QueryAsync<Registration>(
            RegistrationQueries.GetByCourseId,
            new { CourseId = courseId }
        );
        return results;
    }

    public async Task<bool> IsRegisteredAsync(string studentId, string courseId)
    {
        using var connection = CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            RegistrationQueries.IsRegistered,
            new { StudentId = studentId, CourseId = courseId }
        );
        return count > 0;
    }

    public async Task<string> CreateAsync(Registration registration)
    {
        // Generate GUID if not set
        if (string.IsNullOrEmpty(registration.Id))
        {
            registration.Id = Guid.NewGuid().ToString();
        }

        if (registration.CreatedAt == default)
        {
            registration.CreatedAt = DateTime.UtcNow;
        }

        if (registration.RegistrationDate == default)
        {
            registration.RegistrationDate = DateTime.UtcNow;
        }

        using var connection = CreateConnection();
        await connection.ExecuteAsync(
            RegistrationQueries.Create,
            new
            {
                Id = registration.Id,
                registration.StudentId,
                registration.CourseId,
                registration.RegistrationDate,
                registration.Status,
                registration.IsActive,
                registration.CreatedAt,
                registration.UpdatedAt
            }
        );
        return registration.Id;
    }

    public async Task<bool> UpdateAsync(Registration registration)
    {
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            RegistrationQueries.Update,
            new
            {
                Id = registration.Id,
                registration.Status,
                registration.IsActive,
                UpdatedAt = DateTime.UtcNow
            }
        );
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            RegistrationQueries.Delete,
            new { Id = id, UpdatedAt = DateTime.UtcNow }
        );
        return rowsAffected > 0;
    }

    public async Task<int> GetRegistrationCountAsync(string courseId)
    {
        using var connection = CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            RegistrationQueries.GetRegistrationCount,
            new { CourseId = courseId }
        );
        return count;
    }
}
