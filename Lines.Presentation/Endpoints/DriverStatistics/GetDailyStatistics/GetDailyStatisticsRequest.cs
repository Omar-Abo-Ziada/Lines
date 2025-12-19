using FluentValidation;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetDailyStatistics;

public record GetDailyStatisticsRequest(Guid DriverId);

public class GetDailyStatisticsRequestValidator : AbstractValidator<GetDailyStatisticsRequest>
{
    public GetDailyStatisticsRequestValidator()
    {
        RuleFor(x => x.DriverId)
            .NotEmpty()
            .WithMessage("DriverId is required");
    }
}


