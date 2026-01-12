using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Enums;
using StudentRegistrationSystem.Domain.Interfaces;
using StudentRegistrationSystem.Core.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(IUnitOfWork unitOfWork, ILogger<RegistrationService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<RegistrationDto>> GetByStudentIdAsync(string studentId)
    {
        var registrations = await _unitOfWork.Registrations.GetByStudentIdAsync(studentId);
        return registrations.Select(MapToDto);
    }

    public async Task<IEnumerable<RegistrationDto>> GetActiveByStudentIdAsync(string studentId)
    {
        var registrations = await _unitOfWork.Registrations.GetActiveByStudentIdAsync(studentId);
        return registrations.Select(MapToDto);
    }

    public async Task<RegistrationDto> RegisterAsync(string studentId, string courseId)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            // Check if already registered
            if (await _unitOfWork.Registrations.IsRegisteredAsync(studentId, courseId))
            {
                throw new DuplicateException("Student is already registered for this course");
            }

            // Check if course exists and is active
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }

            if (!course.IsActive)
            {
                throw new BusinessException("Course is not active");
            }

            // Check capacity if MaxCapacity is set
            if (course.MaxCapacity.HasValue)
            {
                var currentCount = await _unitOfWork.Registrations.GetRegistrationCountAsync(courseId);
                if (currentCount >= course.MaxCapacity.Value)
                {
                    throw new BusinessException("Course has reached maximum capacity");
                }
            }

            var registration = new Registration
            {
                Id = TokenGenerator.GenerateToken(),
                StudentId = studentId,
                CourseId = courseId,
                RegistrationDate = DateTimeHelper.UtcNow,
                Status = RegistrationStatus.Registered,
                IsActive = true,
                CreatedAt = DateTimeHelper.UtcNow
            };

            var registrationId = await _unitOfWork.Registrations.CreateAsync(registration);
            _unitOfWork.Commit();

            _logger.LogInformation("Student {StudentId} registered for course {CourseId}", studentId, courseId);

            return MapToDto(registration);
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            _logger.LogError(ex, "Failed to register student {StudentId} for course {CourseId}", studentId, courseId);
            throw;
        }
    }

    public async Task<bool> UnregisterAsync(string studentId, string courseId)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            // Check if registered
            var existingRegistration = await _unitOfWork.Registrations.GetByStudentIdAsync(studentId);
            var registration = existingRegistration.FirstOrDefault(r => r.CourseId == courseId && r.IsActive);

            if (registration == null)
            {
                throw new NotFoundException("Registration not found");
            }

            // Check if semester has started
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }

            if (DateTimeHelper.IsTodayOrPast(course.SemesterStartDate))
            {
                throw new BusinessException("Cannot unregister: Semester has already started");
            }

            // Update registration status to Dropped
            registration.Status = RegistrationStatus.Dropped;
            registration.IsActive = false;
            registration.UpdatedAt = DateTimeHelper.UtcNow;

            var result = await _unitOfWork.Registrations.UpdateAsync(registration);
            _unitOfWork.Commit();

            _logger.LogInformation("Student {StudentId} unregistered from course {CourseId}", studentId, courseId);

            return result;
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            _logger.LogError(ex, "Failed to unregister student {StudentId} from course {CourseId}", studentId, courseId);
            throw;
        }
    }

    public async Task<bool> IsRegisteredAsync(string studentId, string courseId)
    {
        return await _unitOfWork.Registrations.IsRegisteredAsync(studentId, courseId);
    }

    private static RegistrationDto MapToDto(Registration registration)
    {
        return new RegistrationDto
        {
            Id = registration.Id,
            StudentId = registration.StudentId,
            CourseId = registration.CourseId,
            RegistrationDate = registration.RegistrationDate,
            Status = registration.Status.ToString(),
            IsActive = registration.IsActive,
            CreatedAt = registration.CreatedAt,
            UpdatedAt = registration.UpdatedAt
        };
    }
}
