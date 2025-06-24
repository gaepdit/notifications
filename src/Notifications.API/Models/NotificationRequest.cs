using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace Notifications.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record NotificationRequest
{
    [JsonRequired]
    public Guid Id { get; init; }
}
