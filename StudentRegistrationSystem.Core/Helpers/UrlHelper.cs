using Microsoft.AspNetCore.Http;

namespace StudentRegistrationSystem.Core.Helpers;

public static class UrlHelper
{
    public static string GetBaseUrl(HttpContext? httpContext, string fallbackUrl)
    {
        if (httpContext != null)
        {
            var request = httpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host;
            return $"{scheme}://{host}";
        }
        
        return fallbackUrl;
    }
}
