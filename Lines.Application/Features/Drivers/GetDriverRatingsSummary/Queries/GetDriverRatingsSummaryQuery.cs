using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetDriverRatingsSummary.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriverRatingsSummary.Queries;

public record GetDriverRatingsSummaryQuery(Guid DriverId) : IRequest<DriverRatingsSummaryDto?>;

public class GetDriverRatingsSummaryQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Lines.Domain.Models.Trips.Feedback> repository)
    : RequestHandlerBase<GetDriverRatingsSummaryQuery, DriverRatingsSummaryDto?>(parameters)
{
    public async override Task<DriverRatingsSummaryDto?> Handle(GetDriverRatingsSummaryQuery request, CancellationToken cancellationToken)
    {
        // Get all feedbacks for this driver
        var feedbacks = await repository.Get()
            .Where(f => f.ToUserId == request.DriverId)
            .Select(f => f.Rating)
            .ToListAsync(cancellationToken);

        if (!feedbacks.Any())
        {
            return new DriverRatingsSummaryDto
            {
                TotalRates = 0,
                AverageRating = 0,
                Rates = new List<RatingRangeDto>()
            };
        }

        var totalRates = feedbacks.Count;
        var averageRating = feedbacks.Average();

        // Group ratings into ranges
        var rates = new List<RatingRangeDto>
        {
            new() { Range = "moreThan4.5", Count = feedbacks.Count(r => r > 4.5) },
            new() { Range = "4to4.5", Count = feedbacks.Count(r => r >= 4.0 && r <= 4.5) },
            new() { Range = "3.5to4", Count = feedbacks.Count(r => r >= 3.5 && r < 4.0) },
            new() { Range = "lessThan3.5", Count = feedbacks.Count(r => r < 3.5) }
        };

        return new DriverRatingsSummaryDto
        {
            TotalRates = totalRates,
            AverageRating = Math.Round(averageRating, 1),
            Rates = rates
        };
    }
}
