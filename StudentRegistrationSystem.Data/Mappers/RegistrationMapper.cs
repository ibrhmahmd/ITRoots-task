using System;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Data.Mappers;

/// <summary>
/// Mapper for Registration entity using Dapper
/// </summary>
public static class RegistrationMapper
{
    /// <summary>
    /// Maps database row to Registration entity
    /// </summary>
    public static Registration Map(dynamic row)
    {
        return new Registration
        {
            Id = row.Id ?? row.RegistrationId,
            StudentId = row.StudentId,
            CourseId = row.CourseId,
            RegistrationDate = row.RegistrationDate,
            Status = row.Status ?? "Registered",
            IsActive = row.IsActive,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt
        };
    }
}
