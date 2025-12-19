namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public record EditEmergencyNumberRequest(Guid Id ,string Name, string PhoneNumber);

public class EditEmergencyNumberRequestValidator : AbstractValidator<EditEmergencyNumberRequest>
{
    public EditEmergencyNumberRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be a valid international format.");

    }
}