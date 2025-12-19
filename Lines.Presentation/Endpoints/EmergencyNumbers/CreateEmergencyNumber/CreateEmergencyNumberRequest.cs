using FluentValidation;

namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public record CreateEmergencyNumberRequest(string Name, string PhoneNumber);

public class CreateEmergencyNumberRequestValidator : AbstractValidator<CreateEmergencyNumberRequest>
{
    public CreateEmergencyNumberRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be a valid international format.");

    }
}