using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystem.Core.Interfaces;
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

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        var allCourses = await _courseService.GetAllActiveAsync();
        var registrations = await _registrationService.GetByStudentIdAsync(student.Id);

        var registeredCourseIds = registrations.Select(r => r.CourseId).ToList();

        var model = new BrowseCoursesViewModel
        {
            AvailableCourses = allCourses.Where(c => !registeredCourseIds.Contains(c.Id)).Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Credits = c.Credits
            }),
            RegisteredCourses = allCourses.Where(c => registeredCourseIds.Contains(c.Id)).Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Credits = c.Credits
            })
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
