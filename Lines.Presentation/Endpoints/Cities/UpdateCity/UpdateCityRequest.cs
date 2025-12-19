using FluentValidation;

namespace Lines.Presentation.Endpoints.Cities;

public record UpdateCityRequest(Guid Id, string Name);
public class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}