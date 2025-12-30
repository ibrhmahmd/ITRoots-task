using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllActiveAsync();
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(string id);
    Task<CourseDto> CreateAsync(CourseDto courseDto);
    Task<CourseDto> UpdateAsync(CourseDto courseDto);
    Task<bool> DeleteAsync(string id);
}

