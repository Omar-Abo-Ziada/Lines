using FluentValidation;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record UpdateVehicleTypeRequest(Guid Id, string Name, string Description, int Capacity, decimal PerKmCharge, decimal PerMinuteDelayCharge);

public class UpdateVehicleTypeRequestValidator : AbstractValidator<UpdateVehicleTypeRequest>
{
    public UpdateVehicleTypeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Vehicle Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is required");
        RuleFor(x => x.PerKmCharge).NotEmpty().WithMessage("Per Km Charge is required");
        RuleFor(x => x.PerMinuteDelayCharge).NotEmpty().WithMessage("Per Minute Delay Charge is required");
    }
}