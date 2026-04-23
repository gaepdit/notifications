using Notifications.Database;

namespace Notifications.Platform;

internal static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        // Status endpoints
        app.MapGet("/", () => Results.Ok());
        app.MapGet("/health", Repository.DbAvailable);
        app.MapGet("/version", () => Results.Ok(new { AppSettings.Version }));

        // Public endpoints
        app.MapGet("/current", Repository.GetCurrentNotificationsAsync);
        app.MapGet("/future", Repository.GetFutureNotificationsAsync);

        // Admin endpoints; API key required
        app.MapGet("/all", Repository.GetAllNotifications).RequireAuthorization(AppSettings.ApiKeys);
        app.MapPost("/add", Repository.AddNotification).RequireAuthorization(AppSettings.ApiKeys);
        app.MapPost("/deactivate", Repository.DeactivateNotification).RequireAuthorization(AppSettings.ApiKeys);
    }
}
