using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Common.Commands;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record RegisterDriverLicenseOrchestrator(string RegistrationToken, RegisterDriverLicenseDto LicenseInfo) : IRequest<Result<bool>>;

public class RegisterDriverLicenseOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository) 
    : RequestHandlerBase<RegisterDriverLicenseOrchestrator, Result<bool>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    
    public override async Task<Result<bool>> Handle(RegisterDriverLicenseOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing registration
            var registration = _repository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (registration == null)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Registration not found"));
            }

            if (registration.Status != RegistrationStatus.InProgress)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Registration is already completed"));
            }

            // Validate license expiration date
            if (request.LicenseInfo.ExpiryDate <= DateTime.Today)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("License has expired. Please provide a valid license with future expiration date."));
            }

            // Check if license expires within 30 days (warning)
            if (request.LicenseInfo.ExpiryDate <= DateTime.Today.AddDays(30))
            {
                // You might want to log this as a warning or handle it differently
                // For now, we'll allow it but it's good to track
            }

            // Upload license images
            var imageUrls = new List<string>();
            if (request.LicenseInfo.LicenseImages != null && request.LicenseInfo.LicenseImages.Length > 0)
            {
                foreach (var image in request.LicenseInfo.LicenseImages)
                {
                    var uploadResult = await _mediator.Send(new UploadImageOrchestrator(image, "driver-licenses"), cancellationToken);
                    if (!uploadResult.IsSuccess)
                    {
                        return Result<bool>.Failure(uploadResult.Error);
                    }
                    imageUrls.Add(uploadResult.Value);
                }
            }

            // Create license data JSON
            var licenseData = new
            {
                LicenseNumber = request.LicenseInfo.LicenseNumber,
                IssueDate = DateTime.UtcNow,  // Add missing IssueDate
                ExpiryDate = request.LicenseInfo.ExpiryDate,
                LicensePhotoUrls = imageUrls  // ✅ Correct property name
            };

            registration.LicenseData = JsonSerializer.Serialize(licenseData);
            registration.IsLicenseCompleted = true;

            await _repository.UpdateAsync(registration, cancellationToken);
            _repository.SaveChanges();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(DriverErrors.RegisterDriverError($"Failed to process license info: {ex.Message}"));
        }
    }
}