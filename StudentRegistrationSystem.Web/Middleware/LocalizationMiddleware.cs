using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentRegistrationSystem.Web.Middleware;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var language = context.Request.Cookies["Language"] ?? "en";
        
        var culture = new CultureInfo(language);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
