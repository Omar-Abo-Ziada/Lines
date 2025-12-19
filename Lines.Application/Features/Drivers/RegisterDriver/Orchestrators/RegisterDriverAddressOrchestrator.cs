using Lines.Application.Features.Common.Commands;
using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Application.Shared;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Sites;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record RegisterDriverAddressOrchestrator(string RegistrationToken, RegisterDriverAddressDto AddressInfo) : IRequest<Result<bool>>;

public class RegisterDriverAddressOrchestratorHandler(RequestHandlerBaseParameters parameters, 
    IRepository<DriverRegistration> registrationRepository,
    IRepository<City> cityRepository) 
    : RequestHandlerBase<RegisterDriverAddressOrchestrator, Result<bool>>(parameters)
{
    private readonly IRepository<DriverRegistration> _registrationRepository = registrationRepository;
    private readonly IRepository<City> _cityRepository = cityRepository;
    
    public override async Task<Result<bool>> Handle(RegisterDriverAddressOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            var driverRegistration = _registrationRepository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (driverRegistration == null)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Registration not found or expired"));
            }

            // Validate city and check if Limousine badge is required for Zurich
            var city = _cityRepository.Get(c => c.Id == request.AddressInfo.CityId).FirstOrDefault();
            if (city == null)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Invalid city selected"));
            }

            // Check if city is Zurich (case-insensitive, handles both "Zurich" and "Zürich")
            var isZurich = city.Name.Equals("Zurich", StringComparison.OrdinalIgnoreCase) || 
                           city.Name.Equals("Zürich", StringComparison.OrdinalIgnoreCase);

            // If city is Zurich, Limousine badge is required
            if (isZurich && request.AddressInfo.LimousineBadge == null)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Limousine badge is required for Zurich"));
            }

            // Upload limousine badge if provided
            string? limousineBadgeUrl = null;
            if (request.AddressInfo.LimousineBadge != null)
            {
                var uploadResult = await _mediator.Send(new UploadImageOrchestrator(request.AddressInfo.LimousineBadge, "limousine-badges"), cancellationToken);
                if (!uploadResult.IsSuccess)
                {
                    return Result<bool>.Failure(uploadResult.Error);
                }
                limousineBadgeUrl = uploadResult.Value;
            }

            // Update DriverRegistration with address information
            driverRegistration.CityId = request.AddressInfo.CityId;
            driverRegistration.SectorId = request.AddressInfo.SectorId;
            driverRegistration.Address = request.AddressInfo.Address;
            driverRegistration.PostalCode = request.AddressInfo.PostalCode;
            driverRegistration.LimousineBadgeUrl = limousineBadgeUrl;
            driverRegistration.IsAddressCompleted = true;

            await _registrationRepository.UpdateAsync(driverRegistration, cancellationToken);
            _registrationRepository.SaveChanges();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(DriverErrors.RegisterDriverError($"Failed to process address info: {ex.Message}"));
        }
    }
}
