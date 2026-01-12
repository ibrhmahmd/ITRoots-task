using System;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto?> GetByUserIdAsync(string userId)
    {
        var student = await _unitOfWork.Students.GetByUserIdAsync(userId);
        if (student == null) return null;

        return new StudentDto
        {
            Id = student.Id,
            UserId = student.UserId,
            FullName = student.FullName,
            Phone = student.Phone,
            AcademicYear = student.AcademicYear,
            EnrollmentDate = student.EnrollmentDate,
            CreatedAt = student.CreatedAt,
            UpdatedAt = student.UpdatedAt
        };
    }
}
