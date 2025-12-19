using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.UpdateDriverAddress;

public class UpdateDriverAddressRequest
{
    public string Address { get; set; } = string.Empty;
    public Guid CityId { get; set; }
    public Guid? SectorId { get; set; }
    public string PostalCode { get; set; } = string.Empty;
}

public class UpdateDriverAddressRequestValidator : AbstractValidator<UpdateDriverAddressRequest>
{
    public UpdateDriverAddressRequestValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters.");
    }
}
