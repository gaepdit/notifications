using FluentValidation;

namespace Notifications.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record CreateNotificationDto
{
    public required string Message { get; init; }
    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
}

public class CreateNotificationValidator : AbstractValidator<CreateNotificationDto>
{
    public CreateNotificationValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message cannot be empty.")
            .MaximumLength(4000)
            .WithMessage("Message must not be longer than 4000 characters.");
        RuleFor(x => x.DisplayEnd)
            .GreaterThan(x => x.DisplayStart)
            .WithMessage("End date must be after start date.");
        RuleFor(x => x.DisplayEnd)
            .GreaterThan(DateTime.Now)
            .WithMessage("End date must be in the future.");
    }
}
