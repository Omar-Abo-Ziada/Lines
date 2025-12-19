using FluentValidation;
using Lines.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.PersonalInfo;

public class RegisterDriverPersonalInfoRequest
{
    public IFormFile? PersonalPicture { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? CommercialRegistration { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public IdentityType IdentityType { get; set; }
}

public class RegisterDriverPersonalInfoRequestValidator : AbstractValidator<RegisterDriverPersonalInfoRequest>
{
    public RegisterDriverPersonalInfoRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.CompanyName)
            .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.CompanyName));

        RuleFor(x => x.CommercialRegistration)
            .MaximumLength(20).WithMessage("Commercial registration cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.CommercialRegistration));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(BeValidDateOfBirth).WithMessage("Date of birth must be valid and driver must be at least 18 years old");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Must(BeValidPhoneNumber).WithMessage("Phone number must contain only digits, spaces, hyphens, parentheses, and optionally start with +");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.Password).WithMessage("Confirm password must match password");

        RuleFor(x => x.IdentityType)
            .IsInEnum().WithMessage("Identity type must be a valid value");

        RuleFor(x => x.PersonalPicture)
            .Must(BeValidImageFile).WithMessage("Personal picture must be a valid image file (JPG, PNG, JPEG)")
            .When(x => x.PersonalPicture != null);
    }

    private static bool BeValidDateOfBirth(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
            
        return age >= 18 && dateOfBirth <= today;
    }

    private static bool BeValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Remove all non-digit characters except + for validation
        var cleanNumber = Regex.Replace(phoneNumber, @"[^\d+]", "");
        
        // Must contain at least 7 digits and at most 15 digits
        var digitCount = Regex.Matches(cleanNumber, @"\d").Count;
        if (digitCount < 7 || digitCount > 15)
            return false;

        // Check if it starts with + (international format)
        if (cleanNumber.StartsWith("+"))
        {
            // International format: + followed by 7-15 digits
            return Regex.IsMatch(cleanNumber, @"^\+\d{7,15}$");
        }
        else
        {
            // National format: 7-15 digits
            return Regex.IsMatch(cleanNumber, @"^\d{7,15}$");
        }
    }

    private static bool BeValidImageFile(IFormFile? file)
    {
        if (file == null) return true;
        
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var allowedMimeTypes = new[] { "image/jpeg", "image/png" };
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension) && 
               allowedMimeTypes.Contains(file.ContentType) &&
               file.Length <= 5 * 1024 * 1024; // 5MB max
    }
}
