using System;
using Dapper;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Data.Mappers;

public static class CourseMapper
{
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new SemesterTypeHandler());
    }


    public static Course Map(dynamic row)
    {
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
