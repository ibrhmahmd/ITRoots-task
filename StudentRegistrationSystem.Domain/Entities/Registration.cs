using System;

namespace StudentRegistrationSystem.Domain.Entities;

/// <summary>
/// Represents a student's registration for a course
/// </summary>
public class Registration : BaseEntity
{
    /// <summary>
    /// Foreign key to Students table
    /// </summary>
    public string StudentId { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to Courses table
    /// </summary>
    public string CourseId { get; set; } = string.Empty;

    /// <summary>
    /// Date when the student registered for the course
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Registration status (Registered, Dropped, Withdrawn)
    /// </summary>
    public string Status { get; set; } = "Registered";

    /// <summary>
    /// Indicates if the registration is active
    /// </summary>
    public bool IsActive { get; set; }

    // Navigation properties (optional, for ORM scenarios)
    /// <summary>
    /// Related Student entity
    /// </summary>
    public Student? Student { get; set; }

    /// <summary>
    /// Related Course entity
    /// </summary>
    public Course? Course { get; set; }
}
