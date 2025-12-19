using Lines.Domain.Enums;

namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public class DriverRegistrationResumeDto
{
    public string RegistrationToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RegistrationStatus Status { get; set; }
    public bool IsPersonalInfoCompleted { get; set; }
    public bool IsContactInfoCompleted { get; set; }
    public bool IsAddressCompleted { get; set; }
    public bool IsLicenseCompleted { get; set; }
    public bool IsVehicleCompleted { get; set; }
    public bool IsWithdrawalInfoCompleted { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; } = 6;
    public string? NextStep { get; set; }
    public DateTime CreatedDate { get; set; }
}

