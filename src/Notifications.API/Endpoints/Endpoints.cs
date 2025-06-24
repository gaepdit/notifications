using Microsoft.Extensions.Options;
using Notifications.AuthHandlers;
using Notifications.Settings;

namespace Notifications.Endpoints;

internal static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok("OK"));

        app.MapGet("/current", (IOptionsSnapshot<AppSettings> data) => data.Value.Notifications
                .Where(n => n.Active && n.DisplayStart < DateTime.Now && DateTime.Now < n.DisplayEnd)
                .OrderBy(n => n.DisplayStart).ToArray())
            .RequireAuthorization(SchemeType.ApiKey);
    }
}
