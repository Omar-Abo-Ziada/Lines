using FluentValidation;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetDriverOffers;

public record GetDriverOffersRequest(Guid DriverId);

public class GetDriverOffersRequestValidator : AbstractValidator<GetDriverOffersRequest>
{
    public GetDriverOffersRequestValidator()
    {
        RuleFor(x => x.DriverId)
            .NotEmpty()
            .WithMessage("DriverId is required");
    }
}


