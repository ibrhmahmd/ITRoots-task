using System.Collections.Generic;
using StudentRegistrationSystem.Domain.Common;

namespace StudentRegistrationSystem.Core.DTOs;

public class BrowseCoursesDto
{
    public PagedResult<CourseDto> AvailableCourses { get; set; } = new();
    public IEnumerable<CourseDto> RegisteredCourses { get; set; } = new List<CourseDto>();
}
