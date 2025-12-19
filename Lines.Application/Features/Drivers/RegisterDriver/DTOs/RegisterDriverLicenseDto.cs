using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record RegisterDriverLicenseDto(
    string LicenseNumber,
    DateTime ExpiryDate,
    IFormFile[] LicenseImages
);
