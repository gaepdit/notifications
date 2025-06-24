using JetBrains.Annotations;

namespace Notifications.Models;

// API classes
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
internal record Notification
{
    [UsedImplicitly]
    private Notification() { } // Used by ORM.

    public Notification(Guid id) => Id = id;
    public Guid Id { get; }
    public required string Message { get; init; }
    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
    public bool Active { get; init; } = true;
}
