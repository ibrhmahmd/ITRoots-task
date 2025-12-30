using System;
using StudentRegistrationSystem.Core.DTOs;

namespace StudentRegistrationSystem.Web.ViewModels.Courses;

public class CourseDetailsViewModel
{
    public CourseDto Course { get; set; } = new CourseDto();
}
