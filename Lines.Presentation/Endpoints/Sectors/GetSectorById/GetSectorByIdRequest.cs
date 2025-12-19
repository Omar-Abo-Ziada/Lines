using FluentValidation;

namespace Lines.Presentation.Endpoints.Sectors;

public record GetSectorByIdRequest(Guid Id);

public class GetSectorByIdRequestValidator : AbstractValidator<GetSectorByIdRequest>
{
    public GetSectorByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}