using System;
using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationSystem.Web.ViewModels.Courses;

public class EditCourseViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course Code")]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course Name")]
    public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Course Name (Arabic)")]
    public string? CourseNameAr { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Description (Arabic)")]
    public string? DescriptionAr { get; set; }

    [Required]
    [Range(1, 10)]
    [Display(Name = "Credits")]
    public int Credits { get; set; }

    [Required]
    [Range(1, 3)]
    [Display(Name = "Semester")]
    public int Semester { get; set; }

    [Required]
    [Display(Name = "Semester Year")]
    public int SemesterYear { get; set; }

    [Display(Name = "Semester Start Date")]
    [DataType(DataType.Date)]
    public DateTime? SemesterStartDate { get; set; }

    [Display(Name = "Max Capacity")]
    public int? MaxCapacity { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }
}
