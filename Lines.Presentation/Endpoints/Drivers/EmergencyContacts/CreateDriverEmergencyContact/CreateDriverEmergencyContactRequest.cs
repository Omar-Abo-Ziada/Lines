using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.CreateDriverEmergencyContact;

public class CreateDriverEmergencyContactRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    
    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = default!;
}

public class CreateDriverEmergencyContactRequestValidator : AbstractValidator<CreateDriverEmergencyContactRequest>
{
    public CreateDriverEmergencyContactRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");
    }
}

