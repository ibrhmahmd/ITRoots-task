using Microsoft.AspNetCore.Mvc;

namespace StudentRegistrationSystem.Web.Controllers;

public class LanguageController : Controller
{
    [HttpPost]
    public IActionResult SetLanguage(string language, string returnUrl)
    {
        Response.Cookies.Append("Language", language, new Microsoft.AspNetCore.Http.CookieOptions
        {
            Expires = System.DateTimeOffset.UtcNow.AddYears(1)
        });

        return LocalRedirect(returnUrl ?? "/");
    }
}
