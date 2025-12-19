using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Steps;

public class GetRegistrationStepsRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
}

public class GetRegistrationStepsRequestValidator : AbstractValidator<GetRegistrationStepsRequest>
{
    public GetRegistrationStepsRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required")
            .Length(36).WithMessage("Registration token must be a valid GUID");
    }
}
