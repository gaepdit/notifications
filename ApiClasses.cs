namespace Notifications;

// API classes
internal record Notification
{
    public required string Message { get; init; }
    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
}

internal record ApiData
{
    public List<Notification> Notifications { get; } = [];
}
