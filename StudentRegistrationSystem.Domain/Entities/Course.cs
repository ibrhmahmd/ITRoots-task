using System;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Domain.Entities;

public class Course : BaseEntity
{
   
    public string CourseCode { get; set; } = string.Empty;


    public string CourseName { get; set; } = string.Empty;

   
    public string? CourseNameAr { get; set; }

    public string? Description { get; set; }

    public string? DescriptionAr { get; set; }

    /// <summary>
    /// Number of credits for the course
    /// </summary>
    public int Credits { get; set; }

    /// <summary>
    /// Semester (Fall, Spring, Summer)
    /// </summary>
    public Semester Semester { get; set; }

    /// <summary>
    /// Year of the semester (e.g., 2024, 2025)
    /// </summary>
    public int SemesterYear { get; set; }

    /// <summary>
    /// Start date of the semester (required for "unregister only if semester hasn't started" rule)
    /// </summary>
    public DateTime? SemesterStartDate { get; set; }

    /// <summary>
    /// Maximum capacity of students for this course
    /// </summary>
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// Indicates if the course is active
    /// </summary>
    public bool IsActive { get; set; }
}
