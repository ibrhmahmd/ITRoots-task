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

public class StudentRepository : BaseRepository, IStudentRepository
{
    public StudentRepository(IDbConnection connection, IDbTransaction? transaction) : base(connection, transaction)
    {
        StudentMapper.Configure();
    }

    public async Task<Student?> GetByIdAsync(string id)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<Student>(
            StudentQueries.GetById,
            new { Id = id },
            transaction: _transaction
        );
        return result;
    }

    public async Task<Student?> GetByUserIdAsync(string userId)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<Student>(
            StudentQueries.GetByUserId,
            new { UserId = userId },
            transaction: _transaction
        );
        return result;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        var results = await Connection.QueryAsync<Student>(StudentQueries.GetAll, transaction: _transaction);
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

        await Connection.ExecuteAsync(
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
            },
            transaction: _transaction
        );
        return student.Id;
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            StudentQueries.Update,
            new
            {
                Id = student.Id,
                student.FullName,
                student.Phone,
                AcademicYear = (int)student.AcademicYear,
                student.EnrollmentDate,
                UpdatedAt = DateTime.UtcNow
            },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsByUserIdAsync(string userId)
    {
        var count = await Connection.QuerySingleAsync<int>(
            StudentQueries.ExistsByUserId,
            new { UserId = userId },
            transaction: _transaction
        );
        return count > 0;
    }
}

