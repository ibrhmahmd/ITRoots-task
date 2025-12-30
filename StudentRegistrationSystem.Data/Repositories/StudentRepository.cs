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
/// Repository implementation for Student entity using Dapper
/// </summary>
public class StudentRepository : BaseRepository, IStudentRepository
{
    public StudentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
        StudentMapper.Configure();
    }

    public async Task<Student?> GetByIdAsync(string id)
    {
        using var connection = CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Student>(
            StudentQueries.GetById,
            new { Id = id }
        );
        return result;
    }

    public async Task<Student?> GetByUserIdAsync(string userId)
    {
        using var connection = CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Student>(
            StudentQueries.GetByUserId,
            new { UserId = userId }
        );
        return result;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var results = await connection.QueryAsync<Student>(StudentQueries.GetAll);
        return results;
    }

    public async Task<string> CreateAsync(Student student)
    {
        // Generate GUID if not set
        if (string.IsNullOrEmpty(student.Id))
        {
            student.Id = Guid.NewGuid().ToString();
        }

        if (student.CreatedAt == default)
        {
            student.CreatedAt = DateTime.UtcNow;
        }

        using var connection = CreateConnection();
        await connection.ExecuteAsync(
            StudentQueries.Create,
            new
            {
                Id = student.Id,
                student.UserId,
                student.FullName,
                student.Phone,
                AcademicYear = (int)student.AcademicYear,
                student.EnrollmentDate,
                student.CreatedAt,
                student.UpdatedAt
            }
        );
        return student.Id;
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            StudentQueries.Update,
            new
            {
                Id = student.Id,
                student.FullName,
                student.Phone,
                AcademicYear = (int)student.AcademicYear,
                student.EnrollmentDate,
                UpdatedAt = DateTime.UtcNow
            }
        );
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsByUserIdAsync(string userId)
    {
        using var connection = CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            StudentQueries.ExistsByUserId,
            new { UserId = userId }
        );
        return count > 0;
    }
}

