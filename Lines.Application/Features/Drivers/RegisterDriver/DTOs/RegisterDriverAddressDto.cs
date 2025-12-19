using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record RegisterDriverAddressDto(
    Guid CityId,
    Guid? SectorId,        // NEW - replaces Region
    string Address,        // RENAMED from Street
    string PostalCode,
    IFormFile? LimousineBadge  // NEW
);
