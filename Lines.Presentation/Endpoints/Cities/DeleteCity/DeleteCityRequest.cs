using FluentValidation;

namespace Lines.Presentation.Endpoints.Cities;

public record DeleteCityRequest(Guid Id);
public class DeleteCityRequestValidator : AbstractValidator<DeleteCityRequest>
{
    public DeleteCityRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}