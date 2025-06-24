using JetBrains.Annotations;

namespace Notifications.Models;

// API classes
[UsedImplicitly]
internal record Notification
{
    public required string Message { get; init; }
    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
    public bool Active { get; init; } = true;
}
