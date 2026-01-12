using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StudentRegistrationSystem.Core.DTOs;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Core.Helpers;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Entities;
using StudentRegistrationSystem.Domain.Interfaces;
using StudentRegistrationSystem.Core.Interfaces;

namespace StudentRegistrationSystem.Core.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CourseService> _logger;

    public CourseService(IUnitOfWork unitOfWork, ILogger<CourseService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<CourseDto>> GetAllActiveAsync()
    {
        var courses = await _unitOfWork.Courses.GetAllActiveAsync();
        return courses.Select(MapToDto);
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        return courses.Select(MapToDto);
    }

    public async Task<CourseDto?> GetByIdAsync(string id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        return course == null ? null : MapToDto(course);
    }

    public async Task<CourseDto> CreateAsync(CourseDto courseDto)
    {
        // Check if course code already exists
        if (await _unitOfWork.Courses.CourseCodeExistsAsync(courseDto.CourseCode))
        {
            throw new DuplicateException("Course code already exists");
        }

        var course = new Course
        {
            Id = TokenGenerator.GenerateToken(),
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
            CreatedAt = DateTimeHelper.UtcNow
        };

        var courseId = await _unitOfWork.Courses.CreateAsync(course);
        courseDto.Id = courseId;
        
        _logger.LogInformation("Course {CourseCode} created successfully with ID {CourseId}", courseDto.CourseCode, courseId);
        
        return courseDto;
    }

    public async Task<CourseDto> UpdateAsync(CourseDto courseDto)
    {
        var existingCourse = await _unitOfWork.Courses.GetByIdAsync(courseDto.Id);
        if (existingCourse == null)
        {
            throw new NotFoundException("Course not found");
        }

        // Check if course code already exists (excluding current course)
        if (await _unitOfWork.Courses.CourseCodeExistsAsync(courseDto.CourseCode, courseDto.Id))
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
            UpdatedAt = DateTimeHelper.UtcNow
        };

        await _unitOfWork.Courses.UpdateAsync(course);
        courseDto.UpdatedAt = course.UpdatedAt;
        
        _logger.LogInformation("Course {CourseCode} updated successfully", courseDto.CourseCode);
        
        return courseDto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        // Check if course has registrations
        if (await _unitOfWork.Courses.HasRegistrationsAsync(id))
        {
            throw new BusinessException("Cannot delete course with existing registrations");
        }

        return await _unitOfWork.Courses.DeleteAsync(id);
    }

    public async Task<PagedResult<CourseDto>> GetAllPagedAsync(PaginationParameters parameters)
    {
        var pagedResult = await _unitOfWork.Courses.GetAllPagedAsync(parameters);
        return new PagedResult<CourseDto>
        {
            Items = pagedResult.Items.Select(MapToDto),
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };
    }

    public async Task<PagedResult<CourseDto>> GetAllActivePagedAsync(PaginationParameters parameters)
    {
        var pagedResult = await _unitOfWork.Courses.GetAllActivePagedAsync(parameters);
        return new PagedResult<CourseDto>
        {
            Items = pagedResult.Items.Select(MapToDto),
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };
    }

    public async Task<BrowseCoursesDto> GetBrowseCoursesAsync(string studentId, PaginationParameters parameters)
    {
        // 1. Get filtered paged courses (active)
        var pagedCourses = await _unitOfWork.Courses.GetAllActivePagedAsync(parameters);
        
        // 2. Get student registrations
        var registrations = await _unitOfWork.Registrations.GetByStudentIdAsync(studentId);
        var registeredCourseIds = registrations.Select(r => r.CourseId).ToHashSet();

        // 3. Filter available courses (exclude registered)
        // Note: This filters after pagination from DB, which matches original controller logic.
        // Ideally should be db-side filtering, but keeping consistent for now.
        var availableCoursesItems = pagedCourses.Items
            .Where(c => !registeredCourseIds.Contains(c.Id))
            .Select(MapToDto)
            .ToList();

        var availableCoursesPaged = new PagedResult<CourseDto>
        {
            Items = availableCoursesItems,
            TotalCount = pagedCourses.TotalCount, // Note: Total count might be off if we filtered some out
            PageNumber = pagedCourses.PageNumber,
            PageSize = pagedCourses.PageSize
        };

        // 4. Get registered courses details
        var allCourses = await _unitOfWork.Courses.GetAllActiveAsync();
        var registeredCourses = allCourses
            .Where(c => registeredCourseIds.Contains(c.Id))
            .Select(MapToDto)
            .ToList();

        return new BrowseCoursesDto
        {
            AvailableCourses = availableCoursesPaged,
            RegisteredCourses = registeredCourses
        };
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
