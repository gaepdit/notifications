using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Notifications.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record CreateNotification
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(4000)]
    public required string Message { get; init; }

    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
}
