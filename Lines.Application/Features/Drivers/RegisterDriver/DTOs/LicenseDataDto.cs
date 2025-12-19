namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public class LicenseDataDto
{
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public List<string>? LicensePhotoUrls { get; set; } // URLs of uploaded license photos
}
