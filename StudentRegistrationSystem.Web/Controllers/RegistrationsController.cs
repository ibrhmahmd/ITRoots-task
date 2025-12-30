using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystem.Core.Interfaces;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;
using StudentRegistrationSystem.Web.ViewModels.Registrations;

namespace StudentRegistrationSystem.Web.Controllers;

[Authorize]
public class RegistrationsController : Controller
{
    private readonly IRegistrationService _registrationService;
    private readonly ICourseService _courseService;
    private readonly IStudentRepository _studentRepository;

    public RegistrationsController(
        IRegistrationService registrationService,
        ICourseService courseService,
        IStudentRepository studentRepository)
    {
        _registrationService = registrationService;
        _courseService = courseService;
        _studentRepository = studentRepository;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null) return RedirectToAction("Login", "Account");

        var registrations = await _registrationService.GetActiveByStudentIdAsync(student.Id);
        var courses = await _courseService.GetAllActiveAsync();

        var model = new MyCoursesViewModel
        {
            Registrations = registrations.Select(r =>
            {
                var course = courses.FirstOrDefault(c => c.Id == r.CourseId);
                return new RegistrationViewModel
                {
                    Id = r.Id,
                    CourseId = r.CourseId,
                    CourseCode = course?.CourseCode ?? "",
                    CourseName = course?.CourseName ?? "",
                    Credits = course?.Credits ?? 0,
                    RegistrationDate = r.RegistrationDate,
                    Status = r.Status
                };
            })
        };

        return View(model);
    }
}
