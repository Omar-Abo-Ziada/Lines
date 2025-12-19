using Lines.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record RegisterDriverPersonalInfoDto(
    IFormFile? PersonalPicture,
    string FirstName,
    string LastName,
    string? CompanyName,
    string? CommercialRegistration,
    DateTime DateOfBirth,
    string PhoneNumber,
    string Password,
    string ConfirmPassword,
    IdentityType IdentityType
);
