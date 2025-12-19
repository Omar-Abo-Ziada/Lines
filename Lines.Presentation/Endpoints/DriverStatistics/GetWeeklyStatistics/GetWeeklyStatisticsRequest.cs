using FluentValidation;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetWeeklyStatistics;

public record GetWeeklyStatisticsRequest(Guid DriverId, DateTime? FromDate, DateTime? ToDate);

public class GetWeeklyStatisticsRequestValidator : AbstractValidator<GetWeeklyStatisticsRequest>
{
    public GetWeeklyStatisticsRequestValidator()
    {
        RuleFor(x => x.DriverId)
            .NotEmpty()
            .WithMessage("DriverId is required");

        RuleFor(x => x.FromDate)
            .LessThan(x => x.ToDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("FromDate must be before ToDate");
    }
}


