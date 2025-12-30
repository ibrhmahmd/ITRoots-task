using System;
using Dapper;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Data.Mappers;

/// <summary>
/// Mapper for Course entity using Dapper
/// </summary>
public static class CourseMapper
{
    /// <summary>
    /// Configures Dapper type mapping for Course entity
    /// </summary>
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new SemesterTypeHandler());
    }

    /// <summary>
    /// Maps database row to Course entity
    /// </summary>
    public static Course Map(dynamic row)
    {
        // Map Semester int to enum
        Semester semester = row.Semester switch
        {
            1 => Semester.Fall,
            2 => Semester.Spring,
            3 => Semester.Summer,
            _ => Semester.Fall
        };

        return new Course
        {
            Id = row.Id ?? row.CourseId,
            CourseCode = row.CourseCode,
            CourseName = row.CourseName,
            CourseNameAr = row.CourseNameAr,
            Description = row.Description,
            DescriptionAr = row.DescriptionAr,
            Credits = row.Credits,
            Semester = semester,
            SemesterYear = row.SemesterYear,
            SemesterStartDate = row.SemesterStartDate,
            MaxCapacity = row.MaxCapacity,
            IsActive = row.IsActive,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt
        };
    }
}

/// <summary>
/// Type handler for Semester enum
/// </summary>
public class SemesterTypeHandler : Dapper.SqlMapper.TypeHandler<Semester>
{
    public override void SetValue(System.Data.IDbDataParameter parameter, Semester value)
    {
        parameter.Value = (int)value;
    }

    public override Semester Parse(object value)
    {
        if (value == null || value == DBNull.Value) return Semester.Fall;
        
        if (int.TryParse(value.ToString(), out int intValue))
        {
            return (Semester)intValue;
        }

        return Semester.Fall;
    }
}
