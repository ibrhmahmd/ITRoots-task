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

    public int Credits { get; set; }

    public Semester Semester { get; set; }

    public int SemesterYear { get; set; }

    public DateTime? SemesterStartDate { get; set; }

    public int? MaxCapacity { get; set; }

    public bool IsActive { get; set; }
}
