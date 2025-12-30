using System;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Domain.Entities;


public class Student : BaseEntity
{
        
    public string UserId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public AcademicYear AcademicYear { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public User? User { get; set; }
}

