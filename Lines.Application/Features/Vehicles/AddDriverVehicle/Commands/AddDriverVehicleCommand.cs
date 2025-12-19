using Lines.Application.Features.Vehicles.AddDriverVehicle.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using Lines.Domain.Models.License;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Vehicles.AddDriverVehicle.Commands;

public record AddDriverVehicleCommand(Guid userId, AddDriverVehicleDto vehicleDto) : IRequest<AddDriverVehicleResponseDto?>;

public class AddDriverVehicleCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<AddDriverVehicleCommand, AddDriverVehicleResponseDto?>(parameters)
{
    public async override Task<AddDriverVehicleResponseDto?> Handle(AddDriverVehicleCommand request, CancellationToken cancellationToken)
    {
        // Get driver ID from user ID using ApplicationUserService
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return null; // User is not a driver
        }

        // Check if this is the first vehicle for the driver
        var existingVehicleCount = await repository.Get()
            .CountAsync(v => v.DriverId == userDriverIds.DriverId.Value && v.IsActive, cancellationToken);

        // Create new vehicle
        var vehicle = new Vehicle(
            driverId: userDriverIds.DriverId.Value,
            vehicleTypeId: request.vehicleDto.VehicleTypeId,
            make: request.vehicleDto.Name, // Using name as make for now
            model: request.vehicleDto.Model,
            year: request.vehicleDto.Year,
            color: request.vehicleDto.Color ?? "Unknown",
            licensePlate: request.vehicleDto.LicensePlate,
            kmPrice: request.vehicleDto.KmPrice
        );

        // Set as primary if it's the first vehicle
        if (existingVehicleCount == 0)
        {
            vehicle.SetAsPrimary();
        }

        // Add vehicle photos if provided
        if (request.vehicleDto.ImageUrls.Any())
        {
            for (int i = 0; i < request.vehicleDto.ImageUrls.Count; i++)
            {
                var photo = new VehiclePhoto(vehicle, request.vehicleDto.ImageUrls[i], i == 0); // First photo is primary
                vehicle.AddPhoto(photo);
            }
        }

        // Create vehicle license with user-provided data and photos
        if (request.vehicleDto.LicensePhotoUrls.Any())
        {
            // Create license photos using parameterless constructor
            var licensePhotos = new List<LicensePhoto>();
            foreach (var photoUrl in request.vehicleDto.LicensePhotoUrls)
            {
                var licensePhoto = new LicensePhoto
                {
                    PhotoUrl = photoUrl
                };
                licensePhotos.Add(licensePhoto);
            }
            
            // Create the VehicleLicense with the photos
            var vehicleLicense = new VehicleLicense(
                licenseNumber: request.vehicleDto.LicenseNumber,
                issuedAt: request.vehicleDto.LicenseIssueDate,
                expiryAt: request.vehicleDto.LicenseExpiryDate,
                vehicleId: vehicle.Id,
                photos: licensePhotos
            );
            
            // Set the LicenseId for each photo after the license is created
            foreach (var photo in licensePhotos)
            {
                photo.LicenseId = vehicleLicense.Id;
                photo.License = vehicleLicense;
            }
            
            vehicle.License = vehicleLicense;
        }

        // Save vehicle
        await repository.AddAsync(vehicle, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        // Return the created vehicle using projection
        var createdVehicle = await repository.Get()
            .Where(v => v.Id == vehicle.Id)
            .Select(v => new AddDriverVehicleResponseDto
            {
                VehicleId = v.Id,
                Name = request.vehicleDto.Name,
                LicensePlate = v.LicensePlate,
                Model = v.Model,
                Year = v.Year,
                VehicleTypeName = v.VehicleType.Name,
                ImageUrls = v.Photos.Select(p => p.PhotoUrl).ToList(),
                IsPrimary = v.IsPrimary,
                IsActive = v.IsActive,
                IsVerified = v.IsVerified,
                Status = v.Status,
                KmPrice = v.KmPrice,
                Color = request.vehicleDto.Color
            })
            .FirstOrDefaultAsync(cancellationToken);

        return createdVehicle;
    }
}
