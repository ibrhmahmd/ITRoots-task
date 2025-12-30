using System.Security.Claims;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name);
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role);
        return role == UserRole.Admin.ToString();
    }

    public static bool IsStudent(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role);
        return role == UserRole.Student.ToString();
    }
}
