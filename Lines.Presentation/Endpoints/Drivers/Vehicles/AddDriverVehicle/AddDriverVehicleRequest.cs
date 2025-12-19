using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.AddDriverVehicle;

public record AddDriverVehicleRequest(
    string Name,
    Guid VehicleTypeId,
    string Model,
    int Year,
    string LicensePlate,
    decimal KmPrice,
    string? Color,
    IFormFile[]? Images,
    IFormFile[] LicensePhotos,      // NEW: Required license photos
    string LicenseNumber,            // NEW: Required license number
    DateTime LicenseIssueDate,       // NEW: Required issue date
    DateTime LicenseExpiryDate       // NEW: Required expiry date
);

public class AddDriverVehicleRequestValidator : AbstractValidator<AddDriverVehicleRequest>
{
    public AddDriverVehicleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vehicle name is required")
            .MaximumLength(100).WithMessage("Vehicle name cannot exceed 100 characters");

        RuleFor(x => x.VehicleTypeId)
            .NotEmpty().WithMessage("Vehicle type is required");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(100).WithMessage("Model cannot exceed 100 characters");

        RuleFor(x => x.Year)
            .GreaterThan(1900).WithMessage("Year must be greater than 1900")
            .LessThanOrEqualTo(DateTime.Now.Year + 2).WithMessage($"Year cannot be greater than {DateTime.Now.Year + 2}");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required")
            .MaximumLength(20).WithMessage("License plate cannot exceed 20 characters");

        RuleFor(x => x.KmPrice)
            .GreaterThan(0).WithMessage("Km price must be greater than 0");

        RuleFor(x => x.Color)
            .MaximumLength(50).WithMessage("Color cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Color));

        RuleFor(x => x.Images)
            .Must(images => images == null || images.Length <= 5)
            .WithMessage("Maximum 5 images allowed")
            .Must(images => images == null || images.All(img => img.Length <= 5 * 1024 * 1024))
            .WithMessage("Each image must be less than 5MB")
            .Must(images => images == null || images.All(img => 
                img.ContentType == "image/jpeg" || 
                img.ContentType == "image/jpg" || 
                img.ContentType == "image/png"))
            .WithMessage("Only JPEG and PNG images are allowed");

        // License Photos validation
        RuleFor(x => x.LicensePhotos)
            .NotNull().WithMessage("License photos are required")
            .NotEmpty().WithMessage("At least one license photo is required")
            .Must(photos => photos.Length >= 1).WithMessage("At least one license photo is required")
            .Must(photos => photos.Length <= 3).WithMessage("Maximum 3 license photos allowed")
            .Must(photos => photos.All(img => img.Length <= 5 * 1024 * 1024))
            .WithMessage("Each license photo must be less than 5MB")
            .Must(photos => photos.All(img => 
                img.ContentType == "image/jpeg" || 
                img.ContentType == "image/jpg" || 
                img.ContentType == "image/png"))
            .WithMessage("Only JPEG and PNG images are allowed for license photos");

        // License Number validation
        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required")
            .MaximumLength(50).WithMessage("License number cannot exceed 50 characters");

        // License Issue Date validation
        RuleFor(x => x.LicenseIssueDate)
            .NotEmpty().WithMessage("License issue date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("License issue date cannot be in the future");

        // License Expiry Date validation
        RuleFor(x => x.LicenseExpiryDate)
            .NotEmpty().WithMessage("License expiry date is required")
            .GreaterThan(x => x.LicenseIssueDate).WithMessage("License expiry date must be after issue date")
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("License has expired");
    }
}
