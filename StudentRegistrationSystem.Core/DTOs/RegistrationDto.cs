using System;

namespace StudentRegistrationSystem.Core.DTOs;

public class RegistrationDto
{
    public string Id { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string CourseId { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = "Registered";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties 
    public StudentDto? Student { get; set; }
    public CourseDto? Course { get; set; }
}
