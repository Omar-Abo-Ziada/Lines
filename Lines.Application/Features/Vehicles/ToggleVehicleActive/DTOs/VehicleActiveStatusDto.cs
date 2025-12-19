using Lines.Domain.Enums;

namespace Lines.Application.Features.Vehicles.ToggleVehicleActive.DTOs;

public class VehicleActiveStatusDto
{
    public Guid VehicleId { get; set; }
    public bool IsActive { get; set; }
    public string Message { get; set; } = string.Empty;
}
