using Lines.Application.Features.Vehicles.ToggleVehicleActive.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Vehicles.ToggleVehicleActive.Commands;

public record ToggleVehicleActiveCommand(Guid vehicleId, Guid userId) : IRequest<VehicleActiveStatusDto?>;

public class ToggleVehicleActiveCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<ToggleVehicleActiveCommand, VehicleActiveStatusDto?>(parameters)
{
    public async override Task<VehicleActiveStatusDto?> Handle(ToggleVehicleActiveCommand request, CancellationToken cancellationToken)
    {
        // Get driver ID from user ID using ApplicationUserService
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return null; // User is not a driver
        }

        // Get the vehicle and verify ownership
        var vehicle = await repository.Get()
            .FirstOrDefaultAsync(v => v.Id == request.vehicleId && v.DriverId == userDriverIds.DriverId.Value, cancellationToken);

        if (vehicle == null)
        {
            return null; // Vehicle not found or doesn't belong to driver
        }

        // Toggle the active status
        if (vehicle.IsActive)
        {
            vehicle.Deactivate();
        }
        else
        {
            vehicle.Activate();
        }

        // Save changes
        await repository.SaveChangesAsync(cancellationToken);

        // Return the updated status
        return new VehicleActiveStatusDto
        {
            VehicleId = vehicle.Id,
            IsActive = vehicle.IsActive,
            Message = vehicle.IsActive ? "Vehicle activated successfully" : "Vehicle deactivated successfully"
        };
    }
}
