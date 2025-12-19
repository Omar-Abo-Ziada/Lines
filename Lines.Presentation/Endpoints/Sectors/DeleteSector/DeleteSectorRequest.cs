using FluentValidation;

namespace Lines.Presentation.Endpoints.Sectors;

public record DeleteSectorRequest(Guid Id);

public class DeleteSectorRequestValidator : AbstractValidator<DeleteSectorRequest>
{
    public DeleteSectorRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}