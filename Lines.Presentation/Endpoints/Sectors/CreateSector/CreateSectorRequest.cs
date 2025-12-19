using FluentValidation;

namespace Lines.Presentation.Endpoints.Sectors;

public record CreateSectorRequest(string Name, Guid CityId);

public class CreateSectorRequestValidator : AbstractValidator<CreateSectorRequest>
{
    public CreateSectorRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CityId).NotEmpty().WithMessage("CityId is required");
    }
}