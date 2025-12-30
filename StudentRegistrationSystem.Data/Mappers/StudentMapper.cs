using System;
using Dapper;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Data.Mappers;


public static class StudentMapper
{
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new AcademicYearTypeHandler());
    }

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

