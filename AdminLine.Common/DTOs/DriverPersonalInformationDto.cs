namespace AdminLine.Common.DTOs;

public class DriverPersonalInformationDto
{
    public string? CompanyName { get; set; }
    public string? CommercialRegistration { get; set; }
    public string? IdentityType { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? ZipCode { get; set; }
    public string? BankAccountIban { get; set; }
}
