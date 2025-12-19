using FluentValidation;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record CreateVehicleTypeRequest(string Name, string Description, int Capacity, decimal PerKmCharge, decimal PerMinuteDelayCharge, decimal AverageSpeedKmPerHour);

public class CreateVehicleTypeRequestValidator : AbstractValidator<CreateVehicleTypeRequest>
{
    public CreateVehicleTypeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is required");
        RuleFor(x => x.PerKmCharge).NotEmpty().WithMessage("Per Km Charge is required");
        RuleFor(x => x.PerMinuteDelayCharge).NotEmpty().WithMessage("Per Minute Delay Charge is required");
        RuleFor(x => x.AverageSpeedKmPerHour).GreaterThan(0).WithMessage("Average Speed Km/Hour must be greater than zero");

    }
}