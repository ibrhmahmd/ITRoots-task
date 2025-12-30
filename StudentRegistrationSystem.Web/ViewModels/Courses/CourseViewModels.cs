using System.ComponentModel.DataAnnotations;
using StudentRegistrationSystem.Domain.Common;

namespace StudentRegistrationSystem.Web.ViewModels.Courses;

public class CourseViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course Code")]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course Name")]
    public string CourseName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 10)]
    public int Credits { get; set; }
}

public class BrowseCoursesViewModel
{
    public PagedResult<CourseViewModel> AvailableCourses { get; set; } = new PagedResult<CourseViewModel>();
    public IEnumerable<CourseViewModel> RegisteredCourses { get; set; } = new List<CourseViewModel>();
}
