using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Address;

public class RegisterDriverAddressRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
    public Guid CityId { get; set; }
    public Guid? SectorId { get; set; } // NEW - replaces Region
    public string Address { get; set; } = string.Empty; // RENAMED from Street
    public string PostalCode { get; set; } = string.Empty;
    public IFormFile? LimousineBadge { get; set; } // NEW
}

public class RegisterDriverAddressRequestValidator : AbstractValidator<RegisterDriverAddressRequest>
{
    public RegisterDriverAddressRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City is required");

        RuleFor(x => x.SectorId)
            .NotEmpty().WithMessage("Province/Sector is required")
            .When(x => x.SectorId.HasValue);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required")
            .Matches(@"^\d{4}$").WithMessage("Postal code must be 4 digits");

        RuleFor(x => x.LimousineBadge)
            .Must(BeValidImageFile).WithMessage("Limousine badge must be a valid image file (jpg, jpeg, png)")
            .When(x => x.LimousineBadge != null);
    }

    private static bool BeValidImageFile(IFormFile? file)
    {
        if (file == null) return true; // Optional field

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return allowedExtensions.Contains(fileExtension);
    }
}
