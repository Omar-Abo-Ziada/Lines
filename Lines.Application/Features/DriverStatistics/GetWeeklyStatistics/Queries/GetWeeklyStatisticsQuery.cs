using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverStatistics.GetWeeklyStatistics.Queries;

public record GetWeeklyStatisticsQuery(Guid DriverId, DateTime? FromDate, DateTime? ToDate) : IRequest<Result<WeeklyStatisticsDto>>;

public class GetWeeklyStatisticsQueryHandler : RequestHandlerBase<GetWeeklyStatisticsQuery, Result<WeeklyStatisticsDto>>
{
    private readonly IRepository<Trip> _tripRepository;

    public GetWeeklyStatisticsQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Trip> tripRepository) : base(parameters)
    {
        _tripRepository = tripRepository;
    }

    public override async Task<Result<WeeklyStatisticsDto>> Handle(GetWeeklyStatisticsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Default to current week if dates not provided
            var fromDate = request.FromDate ?? GetStartOfWeek(DateTime.UtcNow);
            var toDate = request.ToDate ?? fromDate.AddDays(7);

            // Get all completed trips for the week
            var trips = await _tripRepository
                .Get(t => t.DriverId == request.DriverId
                       && t.Status == TripStatus.Completed
                       && t.StartedAt != null
                       && t.StartedAt >= fromDate
                       && t.StartedAt < toDate)
                .Include(t => t.Payment)
                    .ThenInclude(p => p!.Earning)
                .ToListAsync(cancellationToken);

            // Group trips by day
            var tripsByDay = trips
                .GroupBy(t => t.StartedAt!.Value.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Initialize result
            var result = new WeeklyStatisticsDto();

            // Process each day of the week
            var daysOfWeek = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            for (int i = 0; i < 7; i++)
            {
                var currentDay = fromDate.AddDays(i);
                var dayName = daysOfWeek[i];

                if (tripsByDay.TryGetValue(currentDay.Date, out var dayTrips))
                {
                    // Distance
                    var distance = dayTrips.Sum(t => t.Distance ?? 0);
                    result.DistancePerDay.Add(new DayValueDto { Day = dayName, Value = distance });
                    result.TotalDistanceWeek += distance;

                    // Income
                    var income = dayTrips.Sum(t => t.Fare);
                    result.IncomePerDay.Add(new DayValueDto { Day = dayName, Value = (double)income });
                    result.TotalIncomeWeek += income;

                    // Trips
                    var tripCount = dayTrips.Count;
                    result.TripsPerDay.Add(new DayValueDto { Day = dayName, Value = tripCount });
                    result.TotalTripsWeek += tripCount;

                    // Net Profit (Income - App Fees)
                    var appFees = dayTrips
                        .Where(t => t.Payment != null && t.Payment.Earning != null)
                        .Sum(t => t.Payment!.Amount - t.Payment.Earning!.Amount);
                    var netProfit = income - appFees;
                    result.NetProfitPerDay.Add(new DayValueDto { Day = dayName, Value = (double)netProfit });
                    result.TotalNetProfitWeek += netProfit;

                    // Total actual working hours (sum of all trips durations)
                    var workingTime = dayTrips
                        .Where(t => t.StartedAt.HasValue && t.EndedAt.HasValue)
                        .Aggregate(TimeSpan.Zero, (sum, t) => sum + (t.EndedAt.Value - t.StartedAt.Value));

                    var workingHours = workingTime.TotalHours;

                    result.WorkingHoursPerDay.Add(new DayValueDto { Day = dayName, Value = workingHours });
                    result.TotalWorkingHoursWeek += workingHours;
                }
                else
                {
                    // No trips for this day
                    result.DistancePerDay.Add(new DayValueDto { Day = dayName, Value = 0 });
                    result.IncomePerDay.Add(new DayValueDto { Day = dayName, Value = 0 });
                    result.TripsPerDay.Add(new DayValueDto { Day = dayName, Value = 0 });
                    result.NetProfitPerDay.Add(new DayValueDto { Day = dayName, Value = 0 });
                    result.WorkingHoursPerDay.Add(new DayValueDto { Day = dayName, Value = 0 });
                }
            }

            // Find peak and min days
            result.PeakDay = result.DistancePerDay.OrderByDescending(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;
            result.MinDay = result.DistancePerDay.OrderBy(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;

            result.PeakIncomeDay = result.IncomePerDay.OrderByDescending(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;
            result.MinIncomeDay = result.IncomePerDay.OrderBy(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;

            result.PeakTripsDay = result.TripsPerDay.OrderByDescending(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;
            result.MinTripsDay = result.TripsPerDay.OrderBy(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;

            result.PeakNetProfitDay = result.NetProfitPerDay.OrderByDescending(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;
            result.MinNetProfitDay = result.NetProfitPerDay.OrderBy(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;

            result.PeakWorkingHoursDay = result.WorkingHoursPerDay.OrderByDescending(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;
            result.MinWorkingHoursDay = result.WorkingHoursPerDay.OrderBy(d => d.Value).FirstOrDefault()?.Day ?? string.Empty;

            // Round totals
            result.TotalDistanceWeek = Math.Round(result.TotalDistanceWeek, 2);
            result.TotalIncomeWeek = Math.Round(result.TotalIncomeWeek, 2);
            result.TotalNetProfitWeek = Math.Round(result.TotalNetProfitWeek, 2);
            result.TotalWorkingHoursWeek = Math.Round(result.TotalWorkingHoursWeek, 2);

            return Result<WeeklyStatisticsDto>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<WeeklyStatisticsDto>.Failure(Error.General);
        }
    }

    private DateTime GetStartOfWeek(DateTime date)
    {
        // Get Monday of the current week
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }
}


