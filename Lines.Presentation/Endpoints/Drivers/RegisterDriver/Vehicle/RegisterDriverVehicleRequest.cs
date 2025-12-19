using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Vehicle;

public class RegisterDriverVehicleRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
    public Guid VehicleTypeId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public IFormFile[] RegistrationPapers { get; set; } = Array.Empty<IFormFile>();
    public decimal KmPrice { get; set; }
}

public class RegisterDriverVehicleRequestValidator : AbstractValidator<RegisterDriverVehicleRequest>
{
    public RegisterDriverVehicleRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");

        RuleFor(x => x.VehicleTypeId)
            .NotEmpty().WithMessage("Vehicle type is required");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Vehicle model is required")
            .MaximumLength(100).WithMessage("Vehicle model cannot exceed 100 characters");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Vehicle year is required")
            .InclusiveBetween(1900, DateTime.Now.Year + 1).WithMessage("Vehicle year must be between 1900 and next year");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Vehicle color is required")
            .MaximumLength(50).WithMessage("Vehicle color cannot exceed 50 characters");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required")
            .MaximumLength(20).WithMessage("License plate cannot exceed 20 characters");

        RuleFor(x => x.RegistrationPapers)
            .NotEmpty().WithMessage("At least one registration paper is required")
            .Must(HaveValidDocuments).WithMessage("All registration papers must be valid files (PDF, JPG, PNG) and not exceed 10MB each");

        RuleFor(x => x.KmPrice)
            .NotEmpty().WithMessage("Price per kilometer is required")
            .GreaterThan(0).WithMessage("Price per kilometer must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Price per kilometer cannot exceed 100");
    }

    private static bool HaveValidDocuments(IFormFile[] files)
    {
        if (files == null || files.Length == 0) return false;

        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
        var allowedMimeTypes = new[] { "application/pdf", "image/jpeg", "image/png" };

        foreach (var file in files)
        {
            if (file == null) return false;
            
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension) || 
                !allowedMimeTypes.Contains(file.ContentType) ||
                file.Length > 10 * 1024 * 1024) // 10MB max
            {
                return false;
            }
        }

        return true;
    }
}
