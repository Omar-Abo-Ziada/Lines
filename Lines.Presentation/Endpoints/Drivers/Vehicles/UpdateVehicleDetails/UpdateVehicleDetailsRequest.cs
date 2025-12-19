using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.UpdateVehicleDetails;

public record UpdateVehicleDetailsRequest(
    string? Name,
    string? Model,
    int? Year,
    string? Color,
    decimal? KmPrice,
    string? LicensePlate,
    Guid? VehicleTypeId,
    IFormFile[]? Images,
    IFormFile[]? LicensePhotos,
    IFormFile[]? RegistrationDocuments
);

public class UpdateVehicleDetailsRequestValidator : AbstractValidator<UpdateVehicleDetailsRequest>
{
    public UpdateVehicleDetailsRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Vehicle name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Model)
            .MaximumLength(100).WithMessage("Model cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Model));

        RuleFor(x => x.Year)
            .GreaterThan(1900).WithMessage("Year must be greater than 1900")
            .LessThanOrEqualTo(DateTime.Now.Year + 2).WithMessage($"Year cannot be greater than {DateTime.Now.Year + 2}")
            .When(x => x.Year.HasValue);

        RuleFor(x => x.Color)
            .MaximumLength(50).WithMessage("Color cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Color));

        RuleFor(x => x.KmPrice)
            .GreaterThan(0).WithMessage("Km price must be greater than 0")
            .When(x => x.KmPrice.HasValue);

        RuleFor(x => x.LicensePlate)
            .MaximumLength(20).WithMessage("License plate cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.LicensePlate));

        RuleFor(x => x.VehicleTypeId)
            .NotEmpty().WithMessage("Vehicle type is required")
            .When(x => x.VehicleTypeId.HasValue);

        // Vehicle Photos Validation
        RuleFor(x => x.Images)
            .Must(images => images == null || images.Length <= 5)
            .WithMessage("Maximum 5 vehicle images allowed")
            .Must(images => images == null || images.All(img => img.Length <= 5 * 1024 * 1024))
            .WithMessage("Each vehicle image must be less than 5MB")
            .Must(images => images == null || images.All(img => 
                img.ContentType == "image/jpeg" || 
                img.ContentType == "image/jpg" || 
                img.ContentType == "image/png"))
            .WithMessage("Only JPEG and PNG vehicle images are allowed");

        // License Photos Validation
        RuleFor(x => x.LicensePhotos)
            .Must(photos => photos == null || photos.Length <= 3)
            .WithMessage("Maximum 3 license photos allowed")
            .Must(photos => photos == null || photos.All(img => img.Length <= 5 * 1024 * 1024))
            .WithMessage("Each license photo must be less than 5MB")
            .Must(photos => photos == null || photos.All(img => 
                img.ContentType == "image/jpeg" || 
                img.ContentType == "image/jpg" || 
                img.ContentType == "image/png"))
            .WithMessage("Only JPEG and PNG license photos are allowed");

        // Registration Documents Validation
        RuleFor(x => x.RegistrationDocuments)
            .Must(docs => docs == null || docs.Length <= 3)
            .WithMessage("Maximum 3 registration documents allowed")
            .Must(docs => docs == null || docs.All(doc => doc.Length <= 10 * 1024 * 1024))
            .WithMessage("Each registration document must be less than 10MB")
            .Must(docs => docs == null || docs.All(doc => 
                doc.ContentType == "image/jpeg" || 
                doc.ContentType == "image/jpg" || 
                doc.ContentType == "image/png" ||
                doc.ContentType == "application/pdf"))
            .WithMessage("Only JPEG, PNG, and PDF registration documents are allowed");
    }
}
