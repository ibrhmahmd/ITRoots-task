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

public class RegistrationRepository : BaseRepository, IRegistrationRepository
{
    public RegistrationRepository(IDbConnection connection, IDbTransaction? transaction) : base(connection, transaction)
    {
    }

    public async Task<Registration?> GetByIdAsync(string id)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<Registration>(
            RegistrationQueries.GetById,
            new { Id = id },
            transaction: _transaction
        );
        return result;
    }

    public async Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId)
    {
        var results = await Connection.QueryAsync<Registration>(
            RegistrationQueries.GetByStudentId,
            new { StudentId = studentId },
            transaction: _transaction
        );
        return results;
    }

    public async Task<IEnumerable<Registration>> GetActiveByStudentIdAsync(string studentId)
    {
        var results = await Connection.QueryAsync<Registration>(
            RegistrationQueries.GetActiveByStudentId,
            new { StudentId = studentId },
            transaction: _transaction
        );
        return results;
    }

    public async Task<IEnumerable<Registration>> GetByCourseIdAsync(string courseId)
    {
        var results = await Connection.QueryAsync<Registration>(
            RegistrationQueries.GetByCourseId,
            new { CourseId = courseId },
            transaction: _transaction
        );
        return results;
    }

    public async Task<bool> IsRegisteredAsync(string studentId, string courseId)
    {
        var count = await Connection.QuerySingleAsync<int>(
            RegistrationQueries.IsRegistered,
            new { StudentId = studentId, CourseId = courseId },
            transaction: _transaction
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

        await Connection.ExecuteAsync(
            RegistrationQueries.Create,
            new
            {
                Id = registration.Id,
                registration.StudentId,
                registration.CourseId,
                registration.RegistrationDate,
                Status = registration.Status.ToString(),
                registration.IsActive,
                registration.CreatedAt,
                registration.UpdatedAt
            },
            transaction: _transaction
        );
        return registration.Id;
    }

    public async Task<bool> UpdateAsync(Registration registration)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            RegistrationQueries.Update,
            new
            {
                Id = registration.Id,
                Status = registration.Status.ToString(),
                registration.IsActive,
                UpdatedAt = DateTime.UtcNow
            },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            RegistrationQueries.Delete,
            new { Id = id, UpdatedAt = DateTime.UtcNow },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<int> GetRegistrationCountAsync(string courseId)
    {
        var count = await Connection.QuerySingleAsync<int>(
            RegistrationQueries.GetRegistrationCount,
            new { CourseId = courseId },
            transaction: _transaction
        );
        return count;
    }
}
