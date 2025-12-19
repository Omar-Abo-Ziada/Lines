using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Status;

public class GetDriverRegistrationStatusRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
}

public class GetDriverRegistrationStatusRequestValidator : AbstractValidator<GetDriverRegistrationStatusRequest>
{
    public GetDriverRegistrationStatusRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");
    }
}
