using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Core.Exceptions;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Enums;
using StudentRegistrationSystem.Web.ViewModels.Courses;

namespace StudentRegistrationSystem.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CoursesController : Controller
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var parameters = new PaginationParameters
        {
            PageNumber = page,
            PageSize = pageSize
        };

        var result = await _courseService.GetAllPagedAsync(parameters);
        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var courseDto = new StudentRegistrationSystem.Core.DTOs.CourseDto
        {
            CourseCode = model.CourseCode,
            CourseName = model.CourseName,
            CourseNameAr = model.CourseNameAr,
            Description = model.Description,
            DescriptionAr = model.DescriptionAr,
            Credits = model.Credits,
            Semester = (Semester)model.Semester,
            SemesterYear = model.SemesterYear,
            SemesterStartDate = model.SemesterStartDate,
            MaxCapacity = model.MaxCapacity,
            IsActive = true
        };

        try
        {
            await _courseService.CreateAsync(courseDto);
            TempData["SuccessMessage"] = "Course created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (DuplicateException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null) return NotFound();

        var model = new EditCourseViewModel
        {
            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            CourseNameAr = course.CourseNameAr,
            Description = course.Description,
            DescriptionAr = course.DescriptionAr,
            Credits = course.Credits,
            Semester = (int)course.Semester,
            SemesterYear = course.SemesterYear,
            SemesterStartDate = course.SemesterStartDate,
            MaxCapacity = course.MaxCapacity,
            IsActive = course.IsActive
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditCourseViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var courseDto = new StudentRegistrationSystem.Core.DTOs.CourseDto
        {
            Id = model.Id,
            CourseCode = model.CourseCode,
            CourseName = model.CourseName,
            CourseNameAr = model.CourseNameAr,
            Description = model.Description,
            DescriptionAr = model.DescriptionAr,
            Credits = model.Credits,
            Semester = (Semester)model.Semester,
            SemesterYear = model.SemesterYear,
            SemesterStartDate = model.SemesterStartDate,
            MaxCapacity = model.MaxCapacity,
            IsActive = model.IsActive
        };

        try
        {
            await _courseService.UpdateAsync(courseDto);
            TempData["SuccessMessage"] = "Course updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (DuplicateException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null) return NotFound();
        
        var model = new CourseDetailsViewModel { Course = course };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null)
        {
            TempData["ErrorMessage"] = "Course not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(course);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            await _courseService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Course deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
