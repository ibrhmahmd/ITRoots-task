using System;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Data.Mappers;

public static class RegistrationMapper
{

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
