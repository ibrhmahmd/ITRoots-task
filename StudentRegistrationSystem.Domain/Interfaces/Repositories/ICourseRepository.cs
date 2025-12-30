using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Course entity operations
/// </summary>
public interface ICourseRepository
{
    /// <summary>
    /// Gets a course by ID
    /// </summary>
    Task<Course?> GetByIdAsync(string id);

    /// <summary>
    /// Gets all active courses
    /// </summary>
    Task<IEnumerable<Course>> GetAllActiveAsync();

    /// <summary>
    /// Gets all courses (including inactive)
    /// </summary>
    Task<IEnumerable<Course>> GetAllAsync();

    /// <summary>
    /// Gets courses by semester and year
    /// </summary>
    Task<IEnumerable<Course>> GetBySemesterAsync(string semester, int semesterYear);

    /// <summary>
    /// Creates a new course
    /// </summary>
    Task<string> CreateAsync(Course course);

    /// <summary>
    /// Updates an existing course
    /// </summary>
    Task<bool> UpdateAsync(Course course);

    /// <summary>
    /// Deletes a course (soft delete by setting IsActive = false)
    /// </summary>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Checks if course has any registrations
    /// </summary>
    Task<bool> HasRegistrationsAsync(string courseId);

    /// <summary>
    /// Checks if course code exists
    /// </summary>
    Task<bool> CourseCodeExistsAsync(string courseCode, string? excludeId = null);

    /// <summary>
    /// Gets paginated courses (including inactive)
    /// </summary>
    Task<PagedResult<Course>> GetAllPagedAsync(PaginationParameters parameters);

    /// <summary>
    /// Gets paginated active courses
    /// </summary>
    Task<PagedResult<Course>> GetAllActivePagedAsync(PaginationParameters parameters);

    /// <summary>
    /// Gets total count of all courses
    /// </summary>
    Task<int> GetCountAsync();

    /// <summary>
    /// Gets total count of active courses
    /// </summary>
    Task<int> GetActiveCountAsync();
}
