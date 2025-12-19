using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;
using Lines.Domain.Models.Drivers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lines.Application.Features.Drivers.RegisterDriver.Queries;

public record GetRegistrationStepsQuery(string RegistrationToken) : IRequest<Result<RegistrationStepsStatusDto>>;

public class GetRegistrationStepsQueryHandler : IRequestHandler<GetRegistrationStepsQuery, Result<RegistrationStepsStatusDto>>
{
    private readonly IRepository<DriverRegistration> _registrationRepository;

    public GetRegistrationStepsQueryHandler(IRepository<DriverRegistration> registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }

    public async Task<Result<RegistrationStepsStatusDto>> Handle(GetRegistrationStepsQuery request, CancellationToken cancellationToken)
    {
        var registration = await _registrationRepository
            .Get(r => r.RegistrationToken == request.RegistrationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (registration == null)
        {
            return Result<RegistrationStepsStatusDto>.Failure(
                new Error("Registration.NotFound", "Registration not found", ErrorType.NotFound));
        }

        var steps = new List<RegistrationStepDto>();

        // Step 1: Personal Info
        steps.Add(new RegistrationStepDto
        {
            StepNumber = 1,
            StepName = "Personal Information",
            IsCompleted = registration.IsPersonalInfoCompleted,
            CompletedAt = registration.IsPersonalInfoCompleted ? registration.CreatedDate : null,
            Data = registration.IsPersonalInfoCompleted ? new PersonalInfoStepDataDto
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Email = registration.Email,
                HasCompanyInfo = !string.IsNullOrEmpty(registration.CompanyName),
                HasPhoto = !string.IsNullOrEmpty(registration.PersonalPictureUrl),
                IdentityType = registration.IdentityType?.ToString()
            } : null
        });

        // Step 2: Contact Info
        steps.Add(new RegistrationStepDto
        {
            StepNumber = 2,
            StepName = "Contact Information",
            IsCompleted = registration.IsContactInfoCompleted,
            CompletedAt = null,
            Data = registration.IsContactInfoCompleted ? new ContactInfoStepDataDto
            {
                Email = registration.Email,
                IsEmailVerified = registration.IsEmailVerified,
                HasVerificationCode = !string.IsNullOrEmpty(registration.EmailVerificationCode),
                VerificationExpiry = registration.EmailVerificationExpiry
            } : null
        });

        // Step 3: Address
        steps.Add(new RegistrationStepDto
        {
            StepNumber = 3,
            StepName = "Address",
            IsCompleted = registration.IsAddressCompleted,
            CompletedAt = null,
            Data = registration.IsAddressCompleted ? new AddressStepDataDto
            {
                Address = registration.Address,
                PostalCode = registration.PostalCode,
                HasLimousineBadge = !string.IsNullOrEmpty(registration.LimousineBadgeUrl)
            } : null
        });

        // Step 4: License
        LicenseStepDataDto? licenseData = null;
        if (registration.IsLicenseCompleted && !string.IsNullOrEmpty(registration.LicenseData))
        {
            try
            {
                var licenseJson = JsonSerializer.Deserialize<LicenseDataDto>(registration.LicenseData);
                if (licenseJson != null)
                {
                    licenseData = new LicenseStepDataDto
                    {
                        LicenseNumber = licenseJson.LicenseNumber,
                        ExpiryDate = licenseJson.ExpiryDate,
                        PhotoCount = licenseJson.LicensePhotoUrls?.Count ?? 0
                    };
                }
            }
            catch { }
        }

        steps.Add(new RegistrationStepDto
        {
            StepNumber = 4,
            StepName = "Driver License",
            IsCompleted = registration.IsLicenseCompleted,
            CompletedAt = null,
            Data = licenseData
        });

        // Step 5: Vehicle
        VehicleStepDataDto? vehicleData = null;
        if (registration.IsVehicleCompleted && !string.IsNullOrEmpty(registration.VehicleData))
        {
            try
            {
                var vehicleJson = JsonSerializer.Deserialize<VehicleDataDto>(registration.VehicleData);
                if (vehicleJson != null)
                {
                    vehicleData = new VehicleStepDataDto
                    {
                        Model = vehicleJson.Model,
                        LicensePlate = vehicleJson.LicensePlate,
                        Year = vehicleJson.Year,
                        DocumentCount = vehicleJson.DocumentUrls?.Count ?? 0
                    };
                }
            }
            catch { }
        }

        steps.Add(new RegistrationStepDto
        {
            StepNumber = 5,
            StepName = "Vehicle Information",
            IsCompleted = registration.IsVehicleCompleted,
            CompletedAt = null,
            Data = vehicleData
        });

        // Step 6: Withdrawal
        WithdrawalStepDataDto? withdrawalData = null;
        if (registration.IsWithdrawalInfoCompleted && !string.IsNullOrEmpty(registration.BankAccountData))
        {
            try
            {
                var bankJson = JsonSerializer.Deserialize<BankAccountDataDto>(registration.BankAccountData);
                if (bankJson != null)
                {
                    withdrawalData = new WithdrawalStepDataDto
                    {
                        BankName = bankJson.BankName,
                        AccountHolderName = bankJson.AccountHolderName
                    };
                }
            }
            catch { }
        }

        steps.Add(new RegistrationStepDto
        {
            StepNumber = 6,
            StepName = "Withdrawal Information",
            IsCompleted = registration.IsWithdrawalInfoCompleted,
            CompletedAt = registration.CompletedAt,
            Data = withdrawalData
        });

        // Calculate progress
        var completedSteps = steps.Count(s => s.IsCompleted);
        var progressPercentage = (int)((completedSteps / 6.0) * 100);
        var currentStep = completedSteps < 6 ? completedSteps + 1 : 6;

        var response = new RegistrationStepsStatusDto
        {
            RegistrationToken = registration.RegistrationToken,
            OverallStatus = registration.Status.ToString(),
            CompletedSteps = completedSteps,
            TotalSteps = 6,
            ProgressPercentage = progressPercentage,
            CurrentStep = currentStep,
            Steps = steps
        };

        return Result<RegistrationStepsStatusDto>.Success(response);
    }
}
