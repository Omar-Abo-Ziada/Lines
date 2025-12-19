using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.GetDriverVehicles;

public record GetDriverVehiclesRequest;

public class GetDriverVehiclesRequestValidator : AbstractValidator<GetDriverVehiclesRequest>
{
    public GetDriverVehiclesRequestValidator()
    {
        // No validation needed - uses authenticated user ID from token
    }
}
