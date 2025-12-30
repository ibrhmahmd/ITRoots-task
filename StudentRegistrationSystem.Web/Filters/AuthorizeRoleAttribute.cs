using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentRegistrationSystem.Domain.Enums;

namespace StudentRegistrationSystem.Web.Filters;

public class AuthorizeRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly UserRole _requiredRole;

    public AuthorizeRoleAttribute(UserRole role)
    {
        _requiredRole = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        var role = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (role != _requiredRole.ToString())
        {
            context.Result = new ForbidResult();
        }
    }
}
