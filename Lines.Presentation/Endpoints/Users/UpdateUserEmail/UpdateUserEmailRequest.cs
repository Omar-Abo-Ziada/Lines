namespace Lines.Presentation.Endpoints.Users.UpdateUserEmail
{
    public record UpdateUserEmailRequest(string CurrentEmail, string NewEmail);

    public class UpdateUserEmailRequestValidator : AbstractValidator<UpdateUserEmailRequest>
    {
        public UpdateUserEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .EmailAddress().WithMessage("Email must be a valid email address")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.NewEmail));
        }

    }
}