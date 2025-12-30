using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(string id);

    Task<IEnumerable<Course>> GetAllActiveAsync();

    Task<IEnumerable<Course>> GetAllAsync();

    Task<IEnumerable<Course>> GetBySemesterAsync(string semester, int semesterYear);

    Task<string> CreateAsync(Course course);

    Task<bool> UpdateAsync(Course course);

    Task<bool> DeleteAsync(string id);

    Task<bool> HasRegistrationsAsync(string courseId);

    Task<bool> CourseCodeExistsAsync(string courseCode, string? excludeId = null);

    Task<PagedResult<Course>> GetAllPagedAsync(PaginationParameters parameters);

    Task<PagedResult<Course>> GetAllActivePagedAsync(PaginationParameters parameters);

    Task<int> GetCountAsync();

    Task<int> GetActiveCountAsync();
}
