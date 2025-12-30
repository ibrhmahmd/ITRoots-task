using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Domain.Common;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllActiveAsync();
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(string id);
    Task<CourseDto> CreateAsync(CourseDto courseDto);
    Task<CourseDto> UpdateAsync(CourseDto courseDto);
    Task<bool> DeleteAsync(string id);
    Task<PagedResult<CourseDto>> GetAllPagedAsync(PaginationParameters parameters);
    Task<PagedResult<CourseDto>> GetAllActivePagedAsync(PaginationParameters parameters);
}

