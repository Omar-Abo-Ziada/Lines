using Lines.Application.Features.Drivers.GetDriverProfile.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriverProfile.Queries;

public record GetDriverProfileQuery(Guid userId) : IRequest<DriverProfileDto?>;

public class GetDriverProfileQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetDriverProfileQuery, DriverProfileDto?>(parameters)
{
    public async override Task<DriverProfileDto?> Handle(GetDriverProfileQuery request, CancellationToken cancellationToken)
    {
        // Get driver ID from user ID using ApplicationUserService
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return null; // User is not a driver
        }

        // Use projection to avoid EF Core owned entity materialization issues
        var driverProfileDto = await repository.Get()
            .Where(d => d.Id == userDriverIds.DriverId.Value)
            .Select(d => new DriverProfileDto
            {
                FirstName = d.FirstName,
                LastName = d.LastName,
                CompanyName = d.CompanyName,
                CommercialRegistration = d.CommercialRegistration,
                DateOfBirth = d.DateOfBirth,
                IdentityType = d.IdentityType,
                PersonalPictureUrl = d.PersonalPictureUrl,
                LicensePhotoUrls = d.Licenses
                    .Where(l =>l.IsActive)
                    .SelectMany(l => l.Photos)
                    .Select(p => p.PhotoUrl)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return driverProfileDto;
    }
}
