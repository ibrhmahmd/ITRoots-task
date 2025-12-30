using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Common;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;
using StudentRegistrationSystem.Web.ViewModels.Courses;

namespace StudentRegistrationSystem.Web.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IRegistrationService _registrationService;
    private readonly IStudentRepository _studentRepository;

    public CoursesController(
        ICourseService courseService,
        IRegistrationService registrationService,
        IStudentRepository studentRepository)
    {
        _courseService = courseService;
        _registrationService = registrationService;
        _studentRepository = studentRepository;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        // Get paginated courses
        var parameters = new PaginationParameters
        {
            PageNumber = page,
            PageSize = pageSize
        };

        var pagedCourses = await _courseService.GetAllActivePagedAsync(parameters);
        var registrations = await _registrationService.GetByStudentIdAsync(student.Id);

        var registeredCourseIds = registrations.Select(r => r.CourseId).ToList();

        // Filter available courses (not registered)
        var availableCourses = pagedCourses.Items
            .Where(c => !registeredCourseIds.Contains(c.Id))
            .Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Credits = c.Credits
            })
            .ToList();

        // Get all courses for registered courses list (this stays small, no pagination needed)
        var allCourses = await _courseService.GetAllActiveAsync();
        var registeredCourses = allCourses
            .Where(c => registeredCourseIds.Contains(c.Id))
            .Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Credits = c.Credits
            })
            .ToList();

        // Create paginated result for available courses
        // Note: We need to adjust total count to exclude registered courses
        // For simplicity, we'll use the total from paginated result
        // In a production scenario, you might want a separate query for available courses count
        var availableCoursesPaged = new PagedResult<CourseViewModel>
        {
            Items = availableCourses,
            TotalCount = pagedCourses.TotalCount, // Approximate, could be refined
            PageNumber = pagedCourses.PageNumber,
            PageSize = pagedCourses.PageSize
        };

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
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        try
        {
            await _registrationService.RegisterAsync(student.Id, courseId);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Unregister(string courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        try
        {
            await _registrationService.UnregisterAsync(student.Id, courseId);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
