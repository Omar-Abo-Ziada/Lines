using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.Dashboard.GetTrips;

public record GetDriverTripsRequest(
    int? TripStatus,
    DateTime? DateRangeStart,
    DateTime? DateRangeEnd,
    int? PaymentStatus,
    int PageNumber = 1,
    int PageSize = 10
);

public class GetDriverTripsRequestValidator : AbstractValidator<GetDriverTripsRequest>
{
    public GetDriverTripsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.DateRangeStart)
            .LessThan(x => x.DateRangeEnd)
            .When(x => x.DateRangeStart.HasValue && x.DateRangeEnd.HasValue)
            .WithMessage("Start date must be before end date");
    }
}
