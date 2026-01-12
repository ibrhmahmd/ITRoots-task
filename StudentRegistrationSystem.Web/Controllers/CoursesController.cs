using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Web.ViewModels.Courses;

namespace StudentRegistrationSystem.Web.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IRegistrationService _registrationService;
    private readonly IStudentService _studentService;

    public CoursesController(
        ICourseService courseService,
        IRegistrationService registrationService,
        IStudentService studentService)
    {
        _courseService = courseService;
        _registrationService = registrationService;
        _studentService = studentService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentService.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        var parameters = new PaginationParameters
        {
            PageNumber = page,
            PageSize = pageSize
        };

        var browseData = await _courseService.GetBrowseCoursesAsync(student.Id, parameters);

        // Map to ViewModel
        var availableCoursesPaged = new PagedResult<CourseViewModel>
        {
            Items = browseData.AvailableCourses.Items.Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Credits = c.Credits
            }).ToList(),
            TotalCount = browseData.AvailableCourses.TotalCount, 
            PageNumber = browseData.AvailableCourses.PageNumber,
            PageSize = browseData.AvailableCourses.PageSize
        };

        var registeredCourses = browseData.RegisteredCourses.Select(c => new CourseViewModel
        {
            Id = c.Id,
            CourseCode = c.CourseCode,
            CourseName = c.CourseName,
            Credits = c.Credits
        }).ToList();

        var model = new BrowseCoursesViewModel
        {
            AvailableCourses = availableCoursesPaged,
            RegisteredCourses = registeredCourses
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(string courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentService.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        try
        {
            await _registrationService.RegisterAsync(student.Id, courseId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Unregister(string courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentService.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        try
        {
            await _registrationService.UnregisterAsync(student.Id, courseId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
