using FluentValidation;
using System.Text.Json.Serialization;

namespace Notifications.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record DeactivateNotificationDto
{
    [JsonRequired]
    public Guid Id { get; init; }
}

public class DeactivateNotificationValidator : AbstractValidator<Notification>
{
    public DeactivateNotificationValidator()
    {
        RuleFor(x => x.Active)
            .Must(x => x) // Notification.Active must be true.
            .WithMessage("Notification is already inactive.");
        RuleFor(x => x.DisplayEnd)
            .Must(x => x > DateTime.Now)
            .WithMessage("Notification has already expired.");
    }
}
