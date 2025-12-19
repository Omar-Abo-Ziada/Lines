using FluentValidation;

namespace Lines.Presentation.Endpoints.Otps.ValidateOtp
{
    public class ValidateOtpRequest
    {
        public Guid UserId { get; set; }
        public string Otp { get; set; }

    }



    public class ValidateOtpRequestValidator : AbstractValidator<ValidateOtpRequest>
    {
        public ValidateOtpRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("OTP must be 6 digits long.")
                .Matches(@"^\d+$").WithMessage("OTP must contain only digits.");
        }
    }
}
