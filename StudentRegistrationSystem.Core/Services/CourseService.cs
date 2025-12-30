using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;
using StudentRegistrationSystem.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentRegistrationSystem.Core.Services;

/// <summary>
/// Service for course operations
/// </summary>
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseDto>> GetAllActiveAsync()
    {
        var courses = await _courseRepository.GetAllActiveAsync();
        return courses.Select(MapToDto);
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        return courses.Select(MapToDto);
    }

    public async Task<CourseDto?> GetByIdAsync(string id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        return course == null ? null : MapToDto(course);
    }

    public async Task<CourseDto> CreateAsync(CourseDto courseDto)
    {
        // Check if course code already exists
        if (await _courseRepository.CourseCodeExistsAsync(courseDto.CourseCode))
        {
            throw new DuplicateException("Course code already exists");
        }

        var course = new Course
        {
            Id = Guid.NewGuid().ToString(),
            CourseCode = courseDto.CourseCode,
            CourseName = courseDto.CourseName,
            CourseNameAr = courseDto.CourseNameAr,
            Description = courseDto.Description,
            DescriptionAr = courseDto.DescriptionAr,
            Credits = courseDto.Credits,
            Semester = courseDto.Semester,
            SemesterYear = courseDto.SemesterYear,
            SemesterStartDate = courseDto.SemesterStartDate,
            MaxCapacity = courseDto.MaxCapacity,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var courseId = await _courseRepository.CreateAsync(course);
        courseDto.Id = courseId;
        return courseDto;
    }

    public async Task<CourseDto> UpdateAsync(CourseDto courseDto)
    {
        var existingCourse = await _courseRepository.GetByIdAsync(courseDto.Id);
        if (existingCourse == null)
        {
            throw new NotFoundException("Course not found");
        }

        // Check if course code already exists (excluding current course)
        if (await _courseRepository.CourseCodeExistsAsync(courseDto.CourseCode, courseDto.Id))
        {
            throw new DuplicateException("Course code already exists");
        }

        var course = new Course
        {
            Id = courseDto.Id,
            CourseCode = courseDto.CourseCode,
            CourseName = courseDto.CourseName,
            CourseNameAr = courseDto.CourseNameAr,
            Description = courseDto.Description,
            DescriptionAr = courseDto.DescriptionAr,
            Credits = courseDto.Credits,
            Semester = courseDto.Semester,
            SemesterYear = courseDto.SemesterYear,
            SemesterStartDate = courseDto.SemesterStartDate,
            MaxCapacity = courseDto.MaxCapacity,
            IsActive = courseDto.IsActive,
            CreatedAt = existingCourse.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        await _courseRepository.UpdateAsync(course);
        courseDto.UpdatedAt = course.UpdatedAt;
        return courseDto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        // Check if course has registrations
        if (await _courseRepository.HasRegistrationsAsync(id))
        {
            throw new BusinessException("Cannot delete course with existing registrations");
        }

        return await _courseRepository.DeleteAsync(id);
    }

    private static CourseDto MapToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            CourseNameAr = course.CourseNameAr,
            Description = course.Description,
            DescriptionAr = course.DescriptionAr,
            Credits = course.Credits,
            Semester = course.Semester,
            SemesterYear = course.SemesterYear,
            SemesterStartDate = course.SemesterStartDate,
            MaxCapacity = course.MaxCapacity,
            IsActive = course.IsActive,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };
    }
}
