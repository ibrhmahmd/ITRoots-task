using System;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Domain.Entities;

public class Registration : BaseEntity
{
    public string StudentId { get; set; } = string.Empty;

    public string CourseId { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }
    public RegistrationStatus Status { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties (for ORM scenarios)
    public Student? Student { get; set; }

    public Course? Course { get; set; }
}
