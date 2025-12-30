using Microsoft.AspNetCore.Builder;
using StudentRegistrationSystem.Web.Middleware;

namespace StudentRegistrationSystem.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LocalizationMiddleware>();
    }

    public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
