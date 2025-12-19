using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.GetVehicleDetails;

public record GetVehicleDetailsRequest;

public class GetVehicleDetailsRequestValidator : AbstractValidator<GetVehicleDetailsRequest>
{
    public GetVehicleDetailsRequestValidator()
    {
        // No validation needed - vehicleId from route, userId from token
    }
}
