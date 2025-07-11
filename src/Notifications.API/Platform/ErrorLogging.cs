using Mindscape.Raygun4Net.AspNetCore;
using Mindscape.Raygun4Net.Extensions.Logging;

namespace Notifications.Platform;

internal static class ErrorLogging
{
    public static void ConfigureRaygunLogging(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddRaygun(webApplicationBuilder.Configuration);
        webApplicationBuilder.Logging.AddRaygunLogger(options =>
        {
            options.MinimumLogLevel = LogLevel.Warning;
            options.OnlyLogExceptions = false;
        });
    }
}
