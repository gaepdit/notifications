using Microsoft.EntityFrameworkCore;
using Notifications.Database;
using Notifications.Settings;

namespace Notifications.Endpoints;

internal static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok("OK"));

        app.MapGet("/current", (AppDbContext dbContext) => dbContext.Notifications
                .Where(n => n.Active && n.DisplayStart < DateTime.Now && DateTime.Now < n.DisplayEnd)
                .OrderBy(n => n.DisplayStart).ToListAsync())
            .RequireAuthorization(AppSettings.ApiKeys);
    }
}
