using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.UpdatePassword
{
    public class UpdatePasswordRequest
    {
        public Guid UserId { get; set; }
        public string? CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");


            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
                .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from current password.");
        }
    }
}
