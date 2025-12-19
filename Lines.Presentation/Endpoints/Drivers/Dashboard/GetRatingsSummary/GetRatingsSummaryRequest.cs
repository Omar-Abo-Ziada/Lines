using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.Dashboard.GetRatingsSummary;

public record GetRatingsSummaryRequest;

public class GetRatingsSummaryRequestValidator : AbstractValidator<GetRatingsSummaryRequest>
{
    public GetRatingsSummaryRequestValidator()
    {
        // No validation needed - uses authenticated driverId from token
    }
}
