using System;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Core.DTOs;

/// <summary>
/// Data Transfer Object for Student entity
/// </summary>
public class StudentDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public AcademicYear AcademicYear { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

