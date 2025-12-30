using System;
using Dapper;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Data.Mappers;

/// <summary>
/// Mapper for Student entity using Dapper
/// </summary>
public static class StudentMapper
{
    /// <summary>
    /// Configures Dapper type mapping for Student entity
    /// </summary>
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new AcademicYearTypeHandler());
    }

    /// <summary>
    /// Maps database row to Student entity
    /// </summary>
    public static Student Map(dynamic row)
    {
        return new Student
        {
            Id = row.Id ?? row.StudentId,
            UserId = row.UserId,
            FullName = row.FullName,
            Phone = row.Phone,
            AcademicYear = (AcademicYear)Convert.ToInt32(row.AcademicYear),
            EnrollmentDate = row.EnrollmentDate,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt
        };
    }
}

/// <summary>
/// Type handler for AcademicYear enum
/// </summary>
public class AcademicYearTypeHandler : Dapper.SqlMapper.TypeHandler<AcademicYear>
{
    public override void SetValue(System.Data.IDbDataParameter parameter, AcademicYear value)
    {
        parameter.Value = (int)value;
    }

    public override AcademicYear Parse(object value)
    {
        return (AcademicYear)Convert.ToInt32(value);
    }
}

