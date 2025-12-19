using Lines.Application.Features.Vehicles.UpdateVehicleDetails.DTOs;
using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;
using Lines.Application.Features.Vehicles.GetVehicleDetails.Queries;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Lines.Domain.Models.License;

namespace Lines.Application.Features.Vehicles.UpdateVehicleDetails.Commands;

public record UpdateVehicleDetailsCommand(Guid vehicleId, Guid userId, UpdateVehicleDetailsDto updateDto) : IRequest<VehicleDetailsDto?>;

public class UpdateVehicleDetailsCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository, IApplicationUserService applicationUserService, IRepository<VehiclePhoto> vehiclePhotoRepository, IRepository<LicensePhoto> licensePhotoRepository)
    : RequestHandlerBase<UpdateVehicleDetailsCommand, VehicleDetailsDto?>(parameters)
{
    public async override Task<VehicleDetailsDto?> Handle(UpdateVehicleDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get driver ID from user ID using ApplicationUserService
            var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
            if (userDriverIds?.DriverId == null)
            {
                return null; // User is not a driver
            }

            // Get the vehicle and verify ownership
            var vehicle = await repository.Get()
                .Include(v => v.Photos)
                .Include(v => v.License)
                    .ThenInclude(l => l.Photos)
                .FirstOrDefaultAsync(v => v.Id == request.vehicleId && v.DriverId == userDriverIds.DriverId.Value, cancellationToken);

            if (vehicle == null)
            {
                return null; // Vehicle not found or doesn't belong to driver
            }

            bool criticalFieldsChanged = false;

            // Update basic vehicle fields if provided
            if (!string.IsNullOrEmpty(request.updateDto.Name))
            {
                // Note: Name is not a direct field in Vehicle entity, might need to store separately
                // For now, we'll skip this or implement as needed
            }

            if (!string.IsNullOrEmpty(request.updateDto.Model) && request.updateDto.Year.HasValue)
            {
                vehicle.UpdateDetails(request.updateDto.Model, request.updateDto.Year.Value);
            }

            if (request.updateDto.KmPrice.HasValue)
            {
                vehicle.UpdateKmPrice(request.updateDto.KmPrice.Value);
            }

            if (!string.IsNullOrEmpty(request.updateDto.LicensePlate))
            {
                if (vehicle.LicensePlate != request.updateDto.LicensePlate)
                {
                    vehicle.LicensePlate = request.updateDto.LicensePlate;
                    criticalFieldsChanged = true;
                }
            }

            if (request.updateDto.VehicleTypeId.HasValue)
            {
                if (vehicle.VehicleTypeId != request.updateDto.VehicleTypeId.Value)
                {
                    vehicle.VehicleTypeId = request.updateDto.VehicleTypeId.Value;
                    criticalFieldsChanged = true;
                }
            }

            // Handle vehicle photo updates
            if (request.updateDto.ImageUrls.Any())
            {
                // Store photos to delete (create copy to avoid collection modification issues)
                var photosToDelete = vehicle.Photos.ToList();
                
                // Delete each photo using repository
                foreach (var photo in photosToDelete)
                {
                    await vehiclePhotoRepository.DeleteAsync(photo.Id, cancellationToken, isHardDelete: true);
                }
                
                // Save changes AFTER deletion, BEFORE adding new photos
                await repository.SaveChangesAsync(cancellationToken);
                
                // Now clear the navigation collection
                vehicle.Photos.Clear();
                
                // Add new vehicle photos
                for (int i = 0; i < request.updateDto.ImageUrls.Count; i++)
                {
                    var photo = new VehiclePhoto(vehicle, request.updateDto.ImageUrls[i], i == 0); // First photo is primary
                    vehicle.AddPhoto(photo);
                }
            }

            // Handle license photo updates
            if (request.updateDto.LicensePhotoUrls.Any())
            {
                // Check if vehicle has a license
                if (vehicle.License != null)
                {
                    // Store photos to delete
                    var licensePhotosToDelete = vehicle.License.Photos.ToList();
                    
                    // Delete each photo using repository
                    foreach (var photo in licensePhotosToDelete)
                    {
                        await licensePhotoRepository.DeleteAsync(photo.Id, cancellationToken, isHardDelete: true);
                    }
                    
                    // Save changes AFTER deletion, BEFORE adding new photos
                    await repository.SaveChangesAsync(cancellationToken);
                    
                    // Now clear the navigation collection
                    vehicle.License.Photos.Clear();
                    
                    // Add new license photos
                    foreach (var photoUrl in request.updateDto.LicensePhotoUrls)
                    {
                        var licensePhoto = new LicensePhoto(vehicle.License, photoUrl);
                        vehicle.License.AddPhoto(licensePhoto);
                    }
                    
                    // License photos updated - reset verification
                    criticalFieldsChanged = true;
                }
                // If license is null, silently skip the update
                // (Vehicle license photos can only be added after license entity exists)
            }

            // Handle registration document updates
            if (request.updateDto.RegistrationDocumentUrls.Any())
            {
                vehicle.UpdateRegistrationDocuments(request.updateDto.RegistrationDocumentUrls);
            }

            // Reset verification if critical fields changed
            if (criticalFieldsChanged)
            {
                vehicle.SetStatus(VehicleRequestStatus.PendingVerification);
                vehicle.IsVerified = false;
            }

            // Save changes (EF Core change tracking will detect all modifications)
            await repository.SaveChangesAsync(cancellationToken);

            // Return updated vehicle details using GetVehicleDetailsQuery
            var updatedVehicleDetails = await _mediator.Send(new GetVehicleDetailsQuery(request.vehicleId, request.userId), cancellationToken);
            return updatedVehicleDetails;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating vehicle details: {ex.Message}", ex);
        }
    
     
    }
}
