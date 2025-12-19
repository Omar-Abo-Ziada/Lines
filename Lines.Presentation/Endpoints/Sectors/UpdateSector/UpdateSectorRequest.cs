using FluentValidation;

namespace Lines.Presentation.Endpoints.Sectors;

public record UpdateSectorRequest(Guid Id, string Name, Guid CityId);

public class UpdateSectorRequestValidator : AbstractValidator<UpdateSectorRequest>
{
    public UpdateSectorRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CityId).NotEmpty().WithMessage("CityId is required");
    }
}