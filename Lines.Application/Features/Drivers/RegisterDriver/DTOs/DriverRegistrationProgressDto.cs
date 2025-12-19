using Lines.Domain.Enums;

namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record DriverRegistrationProgressDto(
    int CurrentStep,
    bool PersonalInfoCompleted,
    bool ContactInfoCompleted,
    bool AddressCompleted,
    bool LicenseCompleted,
    bool VehicleCompleted,
    bool WithdrawalCompleted,
    RegistrationStatus RegistrationStatus,
    DateTime? ExpiryDate
);
