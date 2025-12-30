using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;
using StudentRegistrationSystem.Core.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly ICourseRepository _courseRepository;

    public RegistrationService(
        IRegistrationRepository registrationRepository,
        ICourseRepository courseRepository)
    {
        _registrationRepository = registrationRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<RegistrationDto>> GetByStudentIdAsync(string studentId)
    {
        var registrations = await _registrationRepository.GetByStudentIdAsync(studentId);
        return registrations.Select(MapToDto);
    }

    public async Task<IEnumerable<RegistrationDto>> GetActiveByStudentIdAsync(string studentId)
    {
        var registrations = await _registrationRepository.GetActiveByStudentIdAsync(studentId);
        return registrations.Select(MapToDto);
    }

    public async Task<RegistrationDto> RegisterAsync(string studentId, string courseId)
    {
        // Check if already registered
        if (await _registrationRepository.IsRegisteredAsync(studentId, courseId))
        {
            throw new DuplicateException("Student is already registered for this course");
        }

        // Check if course exists and is active
        var course = await _courseRepository.GetByIdAsync(courseId);
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
            var currentCount = await _registrationRepository.GetRegistrationCountAsync(courseId);
            if (currentCount >= course.MaxCapacity.Value)
            {
                throw new BusinessException("Course has reached maximum capacity");
            }
        }

        var registration = new Registration
        {
            Id = Guid.NewGuid().ToString(),
            StudentId = studentId,
            CourseId = courseId,
            RegistrationDate = DateTime.UtcNow,
            Status = "Registered",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var registrationId = await _registrationRepository.CreateAsync(registration);
        return MapToDto(registration);
    }

    public async Task<bool> UnregisterAsync(string studentId, string courseId)
    {
        // Check if registered
        var existingRegistration = await _registrationRepository.GetByStudentIdAsync(studentId);
        var registration = existingRegistration.FirstOrDefault(r => r.CourseId == courseId && r.IsActive);

        if (registration == null)
        {
            throw new NotFoundException("Registration not found");
        }

        // Check if semester has started
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new NotFoundException("Course not found");
        }

        if (course.SemesterStartDate.HasValue && course.SemesterStartDate.Value <= DateTime.Today)
        {
            throw new BusinessException("Cannot unregister: Semester has already started");
        }

        // Update registration status to Dropped
        registration.Status = "Dropped";
        registration.IsActive = false;
        registration.UpdatedAt = DateTime.UtcNow;

        return await _registrationRepository.UpdateAsync(registration);
    }

    public async Task<bool> IsRegisteredAsync(string studentId, string courseId)
    {
        return await _registrationRepository.IsRegisteredAsync(studentId, courseId);
    }

    private static RegistrationDto MapToDto(Registration registration)
    {
        return new RegistrationDto
        {
            Id = registration.Id,
            StudentId = registration.StudentId,
            CourseId = registration.CourseId,
            RegistrationDate = registration.RegistrationDate,
            Status = registration.Status,
            IsActive = registration.IsActive,
            CreatedAt = registration.CreatedAt,
            UpdatedAt = registration.UpdatedAt
        };
    }
}
