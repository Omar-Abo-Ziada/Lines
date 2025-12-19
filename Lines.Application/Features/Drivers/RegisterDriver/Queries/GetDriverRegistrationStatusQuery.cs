using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Queries;

public record GetDriverRegistrationStatusQuery(string RegistrationToken) : IRequest<Result<DriverRegistrationStatusDto>>;

public class GetDriverRegistrationStatusQueryHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository) 
    : RequestHandlerBase<GetDriverRegistrationStatusQuery, Result<DriverRegistrationStatusDto>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    
    public override async Task<Result<DriverRegistrationStatusDto>> Handle(GetDriverRegistrationStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Look up DriverRegistration by token
            var registration = _repository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (registration == null)
            {
                return Result<DriverRegistrationStatusDto>.Failure(DriverErrors.RegisterDriverError("Registration not found"));
            }

            // Calculate completed steps
            var completedSteps = 0;
            if (registration.IsPersonalInfoCompleted) completedSteps++;
            if (registration.IsContactInfoCompleted) completedSteps++;
            if (registration.IsAddressCompleted) completedSteps++;
            if (registration.IsLicenseCompleted) completedSteps++;
            if (registration.IsVehicleCompleted) completedSteps++;
            if (registration.IsWithdrawalInfoCompleted) completedSteps++;

            // Determine next step
            string? nextStep = null;
            if (!registration.IsPersonalInfoCompleted) nextStep = "personal-info";
            else if (!registration.IsContactInfoCompleted) nextStep = "contact-info";
            else if (!registration.IsAddressCompleted) nextStep = "address";
            else if (!registration.IsLicenseCompleted) nextStep = "license";
            else if (!registration.IsVehicleCompleted) nextStep = "vehicle";
            else if (!registration.IsWithdrawalInfoCompleted) nextStep = "withdrawal";
            else nextStep = "completed";

            // Map to DTO
            var statusDto = new DriverRegistrationStatusDto
            {
                RegistrationToken = registration.RegistrationToken,
                Status = registration.Status,
                IsPersonalInfoCompleted = registration.IsPersonalInfoCompleted,
                IsContactInfoCompleted = registration.IsContactInfoCompleted,
                IsEmailVerified = registration.IsEmailVerified,
                IsAddressCompleted = registration.IsAddressCompleted,
                IsLicenseCompleted = registration.IsLicenseCompleted,
                IsVehicleCompleted = registration.IsVehicleCompleted,
                IsWithdrawalInfoCompleted = registration.IsWithdrawalInfoCompleted,
                CompletedSteps = completedSteps,
                TotalSteps = 6,
                NextStep = nextStep
            };

            return Result<DriverRegistrationStatusDto>.Success(statusDto);
        }
        catch (Exception ex)
        {
            return Result<DriverRegistrationStatusDto>.Failure(DriverErrors.RegisterDriverError($"Failed to get registration status: {ex.Message}"));
        }
    }
}

