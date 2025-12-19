using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Models.Drivers;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Queries;

public record GetDriverRegistrationByEmailQuery(string Email) : IRequest<Result<DriverRegistrationResumeDto>>;

public class GetDriverRegistrationByEmailQueryHandler : RequestHandlerBase<GetDriverRegistrationByEmailQuery, Result<DriverRegistrationResumeDto>>
{
    private readonly IRepository<DriverRegistration> _repository;

    public GetDriverRegistrationByEmailQueryHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository) 
        : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<DriverRegistrationResumeDto>> Handle(GetDriverRegistrationByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var driverRegistration = _repository.Get(r => r.Email == request.Email).FirstOrDefault();
            
            if (driverRegistration == null)
            {
                return Result<DriverRegistrationResumeDto>.Failure(DriverErrors.RegisterDriverError("No registration found with this email address"));
            }

            // Check if registration is still in progress
            if (driverRegistration.Status != RegistrationStatus.InProgress)
            {
                return Result<DriverRegistrationResumeDto>.Failure(DriverErrors.RegisterDriverError("Registration is no longer in progress"));
            }

            var resumeDto = new DriverRegistrationResumeDto
            {
                RegistrationToken = driverRegistration.RegistrationToken,
                Email = driverRegistration.Email,
                Status = driverRegistration.Status,
                IsPersonalInfoCompleted = driverRegistration.IsPersonalInfoCompleted,
                IsContactInfoCompleted = driverRegistration.IsContactInfoCompleted,
                IsAddressCompleted = driverRegistration.IsAddressCompleted,
                IsLicenseCompleted = driverRegistration.IsLicenseCompleted,
                IsVehicleCompleted = driverRegistration.IsVehicleCompleted,
                IsWithdrawalInfoCompleted = driverRegistration.IsWithdrawalInfoCompleted,
                CompletedSteps = GetCompletedStepsCount(driverRegistration),
                TotalSteps = 6,
                NextStep = GetNextStep(driverRegistration),
                CreatedDate = driverRegistration.CreatedDate
            };

            return Result<DriverRegistrationResumeDto>.Success(resumeDto);
        }
        catch (Exception ex)
        {
            return Result<DriverRegistrationResumeDto>.Failure(DriverErrors.RegisterDriverError($"Failed to retrieve registration: {ex.Message}"));
        }
    }

    private static int GetCompletedStepsCount(DriverRegistration registration)
    {
        int count = 0;
        if (registration.IsPersonalInfoCompleted) count++;
        if (registration.IsContactInfoCompleted) count++;
        if (registration.IsAddressCompleted) count++;
        if (registration.IsLicenseCompleted) count++;
        if (registration.IsVehicleCompleted) count++;
        if (registration.IsWithdrawalInfoCompleted) count++;
        return count;
    }

    private static string GetNextStep(DriverRegistration registration)
    {
        if (!registration.IsPersonalInfoCompleted) return "Personal Info";
        if (!registration.IsContactInfoCompleted) return "Contact Info";
        if (!registration.IsAddressCompleted) return "Address";
        if (!registration.IsLicenseCompleted) return "License";
        if (!registration.IsVehicleCompleted) return "Vehicle";
        if (!registration.IsWithdrawalInfoCompleted) return "Withdrawal Info";
        return "Complete";
    }
}

