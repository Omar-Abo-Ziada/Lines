using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record RegisterDriverVehicleDto(
    Guid VehicleTypeId,
    string Model,
    int Year,
    string Color,
    string LicensePlate,
    IFormFile[] RegistrationPapers,
    decimal KmPrice
);
