namespace Lines.Presentation.Endpoints.Passengers.SubmitReferralCode
{
    public record SubmitReferralCodeRequest(string ReferralCode);

    public class SubmitReferralCodeRequestValidator : AbstractValidator<SubmitReferralCodeRequest>
    {
        public SubmitReferralCodeRequestValidator()
        {
            RuleFor(x => x.ReferralCode)
                .NotEmpty()
                .WithMessage("Referral code must not be empty.")
                .Length(8)
                .WithMessage("Referral code must be 8 characters long.");
        }
    }


}
