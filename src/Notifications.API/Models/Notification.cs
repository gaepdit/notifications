using System.ComponentModel.DataAnnotations;

namespace Notifications.Models;

// API classes
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record Notification
{
    [UsedImplicitly]
    private Notification() { } // Used by ORM.

    public Notification(Guid id) => Id = id;

    public Guid Id { get; }

    [StringLength(4000)]
    public required string Message { get; init; }

    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
    public bool Active { get; set; } = true;

    public void Deactivate() => Active = false;

    public static Notification Create(CreateNotificationDto resource) =>
        new(id: Guid.NewGuid())
        {
            Message = resource.Message,
            DisplayStart = resource.DisplayStart,
            DisplayEnd = resource.DisplayEnd,
        };
}
