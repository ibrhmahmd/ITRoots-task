using System;

namespace StudentRegistrationSystem.Domain.Entities;

public class Registration : BaseEntity
{
    public string StudentId { get; set; } = string.Empty;

    public string CourseId { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }

    public string Status { get; set; } = "Registered";

    public bool IsActive { get; set; }

    // Navigation properties (optional, for ORM scenarios)
    public Student? Student { get; set; }

    public Course? Course { get; set; }
}
