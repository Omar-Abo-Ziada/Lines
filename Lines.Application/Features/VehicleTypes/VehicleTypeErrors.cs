using Lines.Application.Shared;

namespace Lines.Application.Features.VehicleTypes;

public static class VehicleTypeErrors
{
    public static Error VehicleTypeExist => new Error("VEHICLETYPE:EXIST", "Vehicle type exist", ErrorType.Validation);
}