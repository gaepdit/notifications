using Microsoft.EntityFrameworkCore;
using Notifications.Models;

namespace Notifications.Database;

internal static class TestData
{
    public static async Task SeedDataAsync(AppDbContext context)
    {
        if (await context.Notifications.AnyAsync()) return;
        context.Notifications.AddRange(NotificationSeedItems);
        await context.SaveChangesAsync();
    }

    private static Guid CurrentNotificationId { get; } = new("00000000-0000-0000-0000-000000000001");
    public static Guid InactiveNotificationId { get; } = new("00000000-0000-0000-0000-000000000002");
    public static Guid ExpiredNotificationId { get; } = new("00000000-0000-0000-0000-000000000003");
    private static Guid FutureNotificationId { get; } = new("00000000-0000-0000-0000-000000000004");

    public static List<Notification> NotificationSeedItems { get; } =
    [
        new(CurrentNotificationId)
        {
            Message = "Current notification",
            DisplayStart = DateTime.UtcNow.AddDays(-1),
            DisplayEnd = DateTime.UtcNow.AddDays(1).AddHours(1),
        },
        new(InactiveNotificationId)
        {
            Message = "Inactive notification",
            DisplayStart = DateTime.UtcNow.AddDays(-1),
            DisplayEnd = DateTime.UtcNow.AddDays(1).AddHours(1),
            Active = false,
        },
        new(ExpiredNotificationId)
        {
            Message = "Expired notification",
            DisplayStart = DateTime.UtcNow.AddDays(-2),
            DisplayEnd = DateTime.UtcNow.AddDays(-1).AddHours(1),
        },
        new(FutureNotificationId)
        {
            Message = "Future notification",
            DisplayStart = DateTime.UtcNow.AddDays(1),
            DisplayEnd = DateTime.UtcNow.AddDays(2).AddHours(1),
        },
    ];
}
