using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Common.Commands;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record RegisterDriverVehicleOrchestrator(string RegistrationToken, RegisterDriverVehicleDto VehicleInfo) : IRequest<Result<bool>>;

public class RegisterDriverVehicleOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository) 
    : RequestHandlerBase<RegisterDriverVehicleOrchestrator, Result<bool>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    
    public override async Task<Result<bool>> Handle(RegisterDriverVehicleOrchestrator request, CancellationToken cancellationToken)
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

            // Upload vehicle documents
            var documentUrls = new List<string>();
            if (request.VehicleInfo.RegistrationPapers != null && request.VehicleInfo.RegistrationPapers.Length > 0)
            {
                foreach (var document in request.VehicleInfo.RegistrationPapers)
                {
                    var uploadResult = await _mediator.Send(new UploadImageOrchestrator(document, "vehicle-documents"), cancellationToken);
                    if (!uploadResult.IsSuccess)
                    {
                        return Result<bool>.Failure(uploadResult.Error);
                    }
                    documentUrls.Add(uploadResult.Value);
                }
            }

            // Create vehicle data JSON
            var vehicleData = new
            {
                VehicleTypeId = request.VehicleInfo.VehicleTypeId,
                Model = request.VehicleInfo.Model,
                Year = request.VehicleInfo.Year,
                Color = request.VehicleInfo.Color,
                LicensePlate = request.VehicleInfo.LicensePlate,
                KmPrice = request.VehicleInfo.KmPrice,
                DocumentUrls = documentUrls
            };

            registration.VehicleData = JsonSerializer.Serialize(vehicleData);
            registration.IsVehicleCompleted = true;

            await _repository.UpdateAsync(registration, cancellationToken);
            _repository.SaveChanges();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(DriverErrors.RegisterDriverError($"Failed to process vehicle info: {ex.Message}"));
        }
    }
}