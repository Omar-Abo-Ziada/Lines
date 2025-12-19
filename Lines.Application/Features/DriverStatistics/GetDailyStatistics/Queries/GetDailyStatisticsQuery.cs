using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverStatistics.GetDailyStatistics.Queries;

public record GetDailyStatisticsQuery(Guid DriverId) : IRequest<Result<DailyStatisticsDto>>;

public class GetDailyStatisticsQueryHandler : RequestHandlerBase<GetDailyStatisticsQuery, Result<DailyStatisticsDto>>
{
    private readonly IRepository<Trip> _tripRepository;

    public GetDailyStatisticsQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Trip> tripRepository) : base(parameters)
    {
        _tripRepository = tripRepository;
    }

    public override async Task<Result<DailyStatisticsDto>> Handle(GetDailyStatisticsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            // Get all completed trips for today
            var trips = await _tripRepository
                .Get(t => t.DriverId == request.DriverId
                       && t.Status == TripStatus.Completed
                       && t.StartedAt != null
                       && t.StartedAt >= today
                       && t.StartedAt < tomorrow)
                .Include(t => t.Payment)
                    .ThenInclude(p => p!.Earning)
                .ToListAsync(cancellationToken);

            if (!trips.Any())
            {
                return Result<DailyStatisticsDto>.Success(new DailyStatisticsDto
                {
                    DistanceKm = 0,
                    IncomeChf = 0,
                    NetProfitChf = 0,
                    NumberOfTrips = 0,
                    AvgIncomePerTrip = 0,
                    AppRightsChf = 0,
                    Availability = "00:00",
                    AvgProfitLastTrip = 0,
                    TipsChf = 0
                });
            }

            // Calculate metrics
            var totalDistance = trips.Sum(t => t.Distance ?? 0);
            var totalIncome = trips.Sum(t => t.Fare);
            var totalTips = trips.Sum(t => t.Tips);
            var numberOfTrips = trips.Count;

            // Calculate app fees: Payment.Amount - Earning.Amount
            var totalAppFees = trips
                .Where(t => t.Payment != null && t.Payment.Earning != null)
                .Sum(t => t.Payment!.Amount - t.Payment.Earning!.Amount);

            var netProfit = totalIncome - totalAppFees;
            var avgIncomePerTrip = numberOfTrips > 0 ? totalIncome / numberOfTrips : 0;

            // Calculate total actual working time (sum of all trip durations)
            var totalWorkingTime = trips
                .Where(t => t.StartedAt.HasValue && t.EndedAt.HasValue)
                .Aggregate(TimeSpan.Zero, (sum, t) => sum + (t.EndedAt!.Value - t.StartedAt!.Value));

            var availability = $"{(int)totalWorkingTime.TotalHours:D2}:{totalWorkingTime.Minutes:D2}";


            // Average profit per trip (using net profit)
            var avgProfitPerTrip = numberOfTrips > 0 ? netProfit / numberOfTrips : 0;

            var result = new DailyStatisticsDto
            {
                DistanceKm = Math.Round(totalDistance, 2),
                IncomeChf = Math.Round(totalIncome, 2),
                NetProfitChf = Math.Round(netProfit, 2),
                NumberOfTrips = numberOfTrips,
                AvgIncomePerTrip = Math.Round(avgIncomePerTrip, 2),
                AppRightsChf = Math.Round(totalAppFees, 2),
                Availability = availability,
                AvgProfitLastTrip = Math.Round(avgProfitPerTrip, 2),
                TipsChf = Math.Round(totalTips, 2)
            };

            return Result<DailyStatisticsDto>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<DailyStatisticsDto>.Failure(Error.General);
        }
    }
}


