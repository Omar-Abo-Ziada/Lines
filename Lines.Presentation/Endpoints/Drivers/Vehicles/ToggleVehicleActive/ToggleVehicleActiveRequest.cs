using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.ToggleVehicleActive;

public record ToggleVehicleActiveRequest;

public class ToggleVehicleActiveRequestValidator : AbstractValidator<ToggleVehicleActiveRequest>
{
    public ToggleVehicleActiveRequestValidator()
    {
        // No validation needed - vehicleId from route, userId from token
    }
}
