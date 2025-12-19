namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public class RegistrationStepDto
{
    public int StepNumber { get; set; }
    public string StepName { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public object? Data { get; set; } // Step-specific data summary
}

public class RegistrationStepsStatusDto
{
    public string RegistrationToken { get; set; } = string.Empty;
    public string OverallStatus { get; set; } = string.Empty; // InProgress, PendingReview, Verified, Rejected
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; } = 6;
    public int ProgressPercentage { get; set; }
    public int CurrentStep { get; set; } // Next step to complete
    public List<RegistrationStepDto> Steps { get; set; } = new();
}

// Step-specific data DTOs
public class PersonalInfoStepDataDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool HasCompanyInfo { get; set; }
    public bool HasPhoto { get; set; }
    public string? IdentityType { get; set; }
}

public class ContactInfoStepDataDto
{
    public string? Email { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool HasVerificationCode { get; set; }
    public DateTime? VerificationExpiry { get; set; }
}

public class AddressStepDataDto
{
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public bool HasLimousineBadge { get; set; }
}

public class LicenseStepDataDto
{
    public string? LicenseNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int PhotoCount { get; set; }
}

public class VehicleStepDataDto
{
    public string? Model { get; set; }
    public string? LicensePlate { get; set; }
    public int Year { get; set; }
    public int DocumentCount { get; set; }
}

public class WithdrawalStepDataDto
{
    public string? BankName { get; set; }
    public string? AccountHolderName { get; set; }
}
