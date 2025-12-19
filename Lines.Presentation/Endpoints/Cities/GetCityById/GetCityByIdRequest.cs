using FluentValidation;

namespace Lines.Presentation.Endpoints.Cities;

public record GetCityByIdRequest(Guid Id);
public class GetCityByIdCityRequestValidator : AbstractValidator<GetCityByIdRequest>
{
    public GetCityByIdCityRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}