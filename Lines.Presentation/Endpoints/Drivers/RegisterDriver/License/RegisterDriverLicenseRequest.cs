using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.License;

public class RegisterDriverLicenseRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public IFormFile[] LicenseImages { get; set; } = Array.Empty<IFormFile>();
}

public class RegisterDriverLicenseRequestValidator : AbstractValidator<RegisterDriverLicenseRequest>
{
    public RegisterDriverLicenseRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required")
            .MaximumLength(50).WithMessage("License number cannot exceed 50 characters");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage("Expiry date is required")
            .Must(BeValidExpiryDate).WithMessage("License must not be expired");

        RuleFor(x => x.LicenseImages)
            .NotEmpty().WithMessage("At least one license image is required")
            .Must(HaveValidImages).WithMessage("All license images must be valid image files (JPG, PNG, JPEG) and not exceed 5MB each");
    }

    private static bool BeValidExpiryDate(DateTime expiryDate)
    {
        return expiryDate > DateTime.Today;
    }

    private static bool HaveValidImages(IFormFile[] files)
    {
        if (files == null || files.Length == 0) return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var allowedMimeTypes = new[] { "image/jpeg", "image/png" };

        foreach (var file in files)
        {
            if (file == null) return false;
            
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension) || 
                !allowedMimeTypes.Contains(file.ContentType) ||
                file.Length > 5 * 1024 * 1024) // 5MB max
            {
                return false;
            }
        }

        return true;
    }
}
