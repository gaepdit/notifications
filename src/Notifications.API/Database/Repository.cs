using Microsoft.EntityFrameworkCore;
using Notifications.Models;

namespace Notifications.Database;

internal static class Repository
{
    // Read

    public static Task<List<Notification>> GetCurrentNotificationsAsync(AppDbContext db) =>
        db.Notifications
            .Where(n => n.Active && n.DisplayStart < DateTime.Now && DateTime.Now < n.DisplayEnd)
            .OrderBy(n => n.DisplayStart).ThenBy(n => n.DisplayEnd).ToListAsync();

    public static Task<List<Notification>> GetFutureNotificationsAsync(AppDbContext db) =>
        db.Notifications
            .Where(n => n.Active && DateTime.Now < n.DisplayStart)
            .OrderBy(n => n.DisplayStart).ThenBy(n => n.DisplayEnd).ToListAsync();

    public static Task<List<Notification>> GetAllNotifications(AppDbContext db) =>
        db.Notifications
            .OrderBy(n => n.DisplayStart).ThenBy(n => n.DisplayEnd).ToListAsync();

    // Write

    public static async Task<IResult> AddNotification(CreateNotification resource, AppDbContext db)
    {
        var notification = Notification.Create(resource);
        await db.Notifications.AddAsync(notification);
        await db.SaveChangesAsync();
        return Results.Ok(notification);
    }

    public static async Task<IResult> DeactivateNotification(NotificationRequest resource, AppDbContext db)
    {
        var notification = await db.Notifications.FirstOrDefaultAsync(n => n.Id == resource.Id);
        if (notification == null) return Results.NotFound("Notification ID not found.");
        if (!notification.Active) return Results.BadRequest("Notification is already inactive.");
        if (DateTime.Now > notification.DisplayEnd) return Results.BadRequest("Notification has already expired.");

        notification.Deactivate();
        await db.SaveChangesAsync();
        return Results.Ok(notification);
    }
}
