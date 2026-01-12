using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using StudentRegistrationSystem.Data.Mappers;
using StudentRegistrationSystem.Data.Queries;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

public class CourseRepository : BaseRepository, ICourseRepository
{
    public CourseRepository(IDbConnection connection, IDbTransaction? transaction) : base(connection, transaction)
    {
        CourseMapper.Configure();
    }

    public async Task<Course?> GetByIdAsync(string id)
    {
        var result = await Connection.QueryFirstOrDefaultAsync<Course>(
            CourseQueries.GetById,
            new { Id = id },
            transaction: _transaction
        );
        return result;
    }

    public async Task<IEnumerable<Course>> GetAllActiveAsync()
    {
        var results = await Connection.QueryAsync<Course>(CourseQueries.GetAllActive, transaction: _transaction);
        return results;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        var results = await Connection.QueryAsync<Course>(CourseQueries.GetAll, transaction: _transaction);
        return results;
    }

    public async Task<IEnumerable<Course>> GetBySemesterAsync(string semester, int semesterYear)
    {
        var results = await Connection.QueryAsync<Course>(
            CourseQueries.GetBySemester,
            new { Semester = semester, SemesterYear = semesterYear },
            transaction: _transaction
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

        await Connection.ExecuteAsync(
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
            },
            transaction: _transaction
        );
        return course.Id;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        var rowsAffected = await Connection.ExecuteAsync(
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
            },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var rowsAffected = await Connection.ExecuteAsync(
            CourseQueries.Delete,
            new { Id = id, UpdatedAt = DateTime.UtcNow },
            transaction: _transaction
        );
        return rowsAffected > 0;
    }

    public async Task<bool> HasRegistrationsAsync(string courseId)
    {
        var count = await Connection.QuerySingleAsync<int>(
            CourseQueries.HasRegistrations,
            new { CourseId = courseId },
            transaction: _transaction
        );
        return count > 0;
    }

    public async Task<bool> CourseCodeExistsAsync(string courseCode, string? excludeId = null)
    {
        var count = await Connection.QuerySingleAsync<int>(
            CourseQueries.CourseCodeExists,
            new { CourseCode = courseCode, ExcludeId = excludeId },
            transaction: _transaction
        );
        return count > 0;
    }

    public async Task<PagedResult<Course>> GetAllPagedAsync(PaginationParameters parameters)
    {
        parameters.Validate();

        // Get total count
        var totalCount = await Connection.QuerySingleAsync<int>(CourseQueries.GetCount, transaction: _transaction);

        // Get paginated results
        var items = await Connection.QueryAsync<Course>(
            CourseQueries.GetAllPaged,
            new { Offset = parameters.Skip, PageSize = parameters.Take },
            transaction: _transaction
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

        // Get total count
        var totalCount = await Connection.QuerySingleAsync<int>(CourseQueries.GetActiveCount, transaction: _transaction);

        // Get paginated results
        var items = await Connection.QueryAsync<Course>(
            CourseQueries.GetAllActivePaged,
            new { Offset = parameters.Skip, PageSize = parameters.Take },
            transaction: _transaction
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
        return await Connection.QuerySingleAsync<int>(CourseQueries.GetCount, transaction: _transaction);
    }

    public async Task<int> GetActiveCountAsync()
    {
        return await Connection.QuerySingleAsync<int>(CourseQueries.GetActiveCount, transaction: _transaction);
    }
}
