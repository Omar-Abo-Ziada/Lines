namespace AdminLine.Common.DTOs;

public class DriverDetailDto : DriverListDto
{
    public string? CompanyName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? PersonalPictureUrl { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public List<VehicleInfoDto> Vehicles { get; set; } = new();
    public List<LicenseInfoDto> Licenses { get; set; } = new();
    public List<BankAccountInfoDto> BankAccounts { get; set; } = new();
}

public class VehicleInfoDto
{
    public Guid Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class LicenseInfoDto
{
    public Guid Id { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public bool IsCurrent { get; set; }
}

public class BankAccountInfoDto
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string Iban { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

