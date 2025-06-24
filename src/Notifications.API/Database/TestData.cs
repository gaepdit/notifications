using Notifications.Models;

namespace Notifications.Database;

internal static class TestData
{
    public static async Task SeedDataAsync(AppDbContext context, CancellationToken token)
    {
        if (context.Notifications.Any()) return;
        context.Notifications.AddRange(NotificationSeedItems);
        await context.SaveChangesAsync(token);
    }

    public static List<Notification> NotificationSeedItems =>
    [
        new(new Guid("00000000-0000-0000-0000-000000000001"))
        {
            Message = "Current notification",
            DisplayStart = DateTime.UtcNow.AddDays(-1),
            DisplayEnd = DateTime.UtcNow.AddDays(1).AddHours(1),
        },
        new(new Guid("00000000-0000-0000-0000-000000000002"))
        {
            Message = "Inactive notification",
            DisplayStart = DateTime.UtcNow.AddDays(-1),
            DisplayEnd = DateTime.UtcNow.AddDays(1).AddHours(1),
            Active = false,
        },
        new(new Guid("00000000-0000-0000-0000-000000000003"))
        {
            Message = "Expired notification",
            DisplayStart = DateTime.UtcNow.AddDays(-2),
            DisplayEnd = DateTime.UtcNow.AddDays(-1).AddHours(1),
        },
        new(new Guid("00000000-0000-0000-0000-000000000004"))
        {
            Message = "Future notification",
            DisplayStart = DateTime.UtcNow.AddDays(1),
            DisplayEnd = DateTime.UtcNow.AddDays(2).AddHours(1),
        },
    ];
}
