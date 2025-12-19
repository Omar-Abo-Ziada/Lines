namespace Lines.Application.Features.Drivers.BankAccounts.DTOs;

public class BankAccountDto
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string IBAN { get; set; } = string.Empty;
    public string SWIFT { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public string? AccountNumber { get; set; }
    public string? BranchName { get; set; }
    public bool IsPrimary { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? CVV { get; set; }
    public Guid DriverId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

