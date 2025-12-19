using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Vehicles.GetVehicleDetails.Queries;

public record GetVehicleDetailsQuery(Guid vehicleId, Guid userId) : IRequest<VehicleDetailsDto?>;

public class GetVehicleDetailsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetVehicleDetailsQuery, VehicleDetailsDto?>(parameters)
{
    public async override Task<VehicleDetailsDto?> Handle(GetVehicleDetailsQuery request, CancellationToken cancellationToken)
    {
        // Get driver ID from user ID using ApplicationUserService
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return null; // User is not a driver
        }

        // Use projection to avoid EF Core owned entity materialization issues
        var vehicleDetails = await repository.Get()
            .Where(v => v.Id == request.vehicleId && v.DriverId == userDriverIds.DriverId.Value)
            .Select(v => new VehicleDetailsDto
            {
                VehicleId = v.Id,
                Name = $"My {v.Model}", // Default naming - can be customized
                LicensePlate = v.LicensePlate,
                Model = v.Model,
                Year = v.Year,
                Brand = v.VehicleType.Name, // Using VehicleType name as brand
                Color = (string?)null, // Color not in current Vehicle model
                KmPrice = v.KmPrice,
                IsActive = v.IsActive,
                IsPrimary = v.IsPrimary,
                IsVerified = v.IsVerified,
                Status = v.Status,
                VehiclePhotos = v.Photos.Select(p => new VehiclePhotoDto
                {
                    PhotoUrl = p.PhotoUrl,
                    Description = p.Description,
                    IsPrimary = p.IsPrimary
                }).ToList(),
                LicenseInfo = v.License != null ? new LicenseInfoDto
                {
                    LicenseNumber = v.License.LicenseNumber,
                    IssueDate = v.License.IssueDate,
                    ExpiryDate = v.License.ExpiryDate,
                    IsValid = v.License.IsValid,
                    Photos = v.License.Photos.Select(lp => lp.PhotoUrl).ToList()
                } : null,
                RegistrationDocuments = new List<string>(), // Will be populated after projection
                VehicleType = new VehicleTypeDto
                {
                    Id = v.VehicleType.Id,
                    Name = v.VehicleType.Name,
                    IconUrl = (string?)null // IconUrl not in current VehicleType model
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        // Get the raw RegistrationDocumentUrls for JSON deserialization
        if (vehicleDetails != null)
        {
            var rawRegistrationUrls = await repository.Get()
                .Where(v => v.Id == request.vehicleId)
                .Select(v => v.RegistrationDocumentUrls)
                .FirstOrDefaultAsync(cancellationToken);

            // Deserialize JSON outside of projection
            if (!string.IsNullOrEmpty(rawRegistrationUrls))
            {
                try
                {
                    vehicleDetails.RegistrationDocuments = System.Text.Json.JsonSerializer.Deserialize<List<string>>(rawRegistrationUrls) ?? new List<string>();
                }
                catch
                {
                    vehicleDetails.RegistrationDocuments = new List<string>();
                }
            }
        }

        return vehicleDetails;
    }
}
