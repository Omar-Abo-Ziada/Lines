using Lines.Application.Features.Vehicles.GetDriverVehicles.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Vehicles.GetDriverVehicles.Queries;

public record GetDriverVehiclesQuery(Guid userId) : IRequest<List<DriverVehicleDto>>;

public class GetDriverVehiclesQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetDriverVehiclesQuery, List<DriverVehicleDto>>(parameters)
{
    public async override Task<List<DriverVehicleDto>> Handle(GetDriverVehiclesQuery request, CancellationToken cancellationToken)
    {
        // Get driver ID from user ID using ApplicationUserService
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return new List<DriverVehicleDto>(); // User is not a driver
        }

        // Use projection to avoid EF Core owned entity materialization issues
        var vehicles = await repository.Get()
            .Where(v => v.DriverId == userDriverIds.DriverId.Value && v.IsActive)
            .Select(v => new DriverVehicleDto
            {
                VehicleId = v.Id,
                Name = $"My {v.Model}", // Default naming - can be customized later
                LicensePlate = v.LicensePlate,
                Model = v.Model,
                Year = v.Year,
                Brand = v.VehicleType.Name, // Using VehicleType name as brand
                VehicleTypeName = v.VehicleType.Name,
                ImageUrl = v.Photos.FirstOrDefault() != null ? v.Photos.FirstOrDefault()!.PhotoUrl : null,
                IsPrimary = v.IsPrimary,
                IsActive = v.IsActive,
                IsVerified = v.IsVerified,
                Status = v.Status,
                KmPrice = v.KmPrice,
                Color = (string?)null // Color not in current Vehicle model
            })
            .ToListAsync(cancellationToken);

        return vehicles;
    }
}
