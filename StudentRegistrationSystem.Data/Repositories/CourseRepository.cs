using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Data.Mappers;
using StudentRegistrationSystem.Data.Queries;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CourseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        CourseMapper.Configure();
    }

    public async Task<Course?> GetByIdAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Course>(
            CourseQueries.GetById,
            new { Id = id }
        );
        return result;
    }

    public async Task<IEnumerable<Course>> GetAllActiveAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<Course>(CourseQueries.GetAllActive);
        return results;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<Course>(CourseQueries.GetAll);
        return results;
    }

    public async Task<IEnumerable<Course>> GetBySemesterAsync(string semester, int semesterYear)
    {
        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<Course>(
            CourseQueries.GetBySemester,
            new { Semester = semester, SemesterYear = semesterYear }
        );
        return results;
    }

    public async Task<string> CreateAsync(Course course)
    {
        // Generate GUID if not set
        if (string.IsNullOrEmpty(course.Id))
        {
            course.Id = Guid.NewGuid().ToString();
        }

        if (course.CreatedAt == default)
        {
            course.CreatedAt = DateTime.UtcNow;
        }

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            CourseQueries.Create,
            new
            {
                Id = course.Id,
                course.CourseCode,
                course.CourseName,
                course.CourseNameAr,
                course.Description,
                course.DescriptionAr,
                course.Credits,
                Semester = (int)course.Semester,
                course.SemesterYear,
                course.SemesterStartDate,
                course.MaxCapacity,
                course.IsActive,
                course.CreatedAt,
                course.UpdatedAt
            }
        );
        return course.Id;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            CourseQueries.Update,
            new
            {
                Id = course.Id,
                course.CourseCode,
                course.CourseName,
                course.CourseNameAr,
                course.Description,
                course.DescriptionAr,
                course.Credits,
                Semester = (int)course.Semester,
                course.SemesterYear,
                course.SemesterStartDate,
                course.MaxCapacity,
                course.IsActive,
                UpdatedAt = DateTime.UtcNow
            }
        );
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            CourseQueries.Delete,
            new { Id = id, UpdatedAt = DateTime.UtcNow }
        );
        return rowsAffected > 0;
    }

    public async Task<bool> HasRegistrationsAsync(string courseId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            CourseQueries.HasRegistrations,
            new { CourseId = courseId }
        );
        return count > 0;
    }

    public async Task<bool> CourseCodeExistsAsync(string courseCode, string? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.QuerySingleAsync<int>(
            CourseQueries.CourseCodeExists,
            new { CourseCode = courseCode, ExcludeId = excludeId }
        );
        return count > 0;
    }

    public async Task<PagedResult<Course>> GetAllPagedAsync(PaginationParameters parameters)
    {
        parameters.Validate();

        using var connection = _connectionFactory.CreateConnection();

        // Get total count
        var totalCount = await connection.QuerySingleAsync<int>(CourseQueries.GetCount);

        // Get paginated results
        var items = await connection.QueryAsync<Course>(
            CourseQueries.GetAllPaged,
            new { Offset = parameters.Skip, PageSize = parameters.Take }
        );

        return new PagedResult<Course>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task<PagedResult<Course>> GetAllActivePagedAsync(PaginationParameters parameters)
    {
        parameters.Validate();

        using var connection = _connectionFactory.CreateConnection();

        // Get total count
        var totalCount = await connection.QuerySingleAsync<int>(CourseQueries.GetActiveCount);

        // Get paginated results
        var items = await connection.QueryAsync<Course>(
            CourseQueries.GetAllActivePaged,
            new { Offset = parameters.Skip, PageSize = parameters.Take }
        );

        return new PagedResult<Course>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task<int> GetCountAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(CourseQueries.GetCount);
    }

    public async Task<int> GetActiveCountAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<int>(CourseQueries.GetActiveCount);
    }
}
