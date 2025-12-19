using AdminLine.Framework.UoW;
using AdminLine.Service.IService;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AdminLine.Service.Service;

public class FinanceService : IFinanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FinanceService> _logger;

    private static readonly TimeSpan CACHE_EXPIRATION = TimeSpan.FromHours(1);

    public FinanceService(
        IUnitOfWork unitOfWork,
        IMemoryCache cache,
        ILogger<FinanceService> logger)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<decimal> GetDriversProfitsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var cacheKey = $"dashboard:drivers_profits:{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";
            
            if (_cache.TryGetValue(cacheKey, out decimal cachedAmount))
            {
                _logger.LogDebug("Cache hit for drivers profits");
                return cachedAmount;
            }

            var earningRepository = _unitOfWork.GetRepository<Earning>();
            var tripRepository = _unitOfWork.GetRepository<Trip>();

            var amount = await Task.Run(async () =>
            {
                // Get all earnings for completed trips in the date range
                var earnings = earningRepository.GetAll()
                    .Include(e => e.Trip)
                    .Where(e => e.Trip != null 
                        && e.Trip.Status == TripStatus.Completed
                        && e.Trip.EndedAt.HasValue
                        && e.Trip.EndedAt.Value >= fromDate
                        && e.Trip.EndedAt.Value <= toDate);

                return await earnings.SumAsync(e => e.Amount);
            });

            _cache.Set(cacheKey, amount, CACHE_EXPIRATION);
            _logger.LogDebug("Cached drivers profits: {Amount} for period {FromDate} to {ToDate}", amount, fromDate, toDate);

            return amount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drivers profits");
            return 0;
        }
    }

    public async Task<decimal> GetAppProfitsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var cacheKey = $"dashboard:app_profits:{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";
            
            if (_cache.TryGetValue(cacheKey, out decimal cachedAmount))
            {
                _logger.LogDebug("Cache hit for app profits");
                return cachedAmount;
            }

            var paymentRepository = _unitOfWork.GetRepository<Payment>();
            var earningRepository = _unitOfWork.GetRepository<Earning>();

            var amount = await Task.Run(async () =>
            {
                // Get all payments with earnings for completed trips in the date range
                // Note: Payment.EarningId is nullable, so we filter for payments that have earnings
                var payments = paymentRepository.GetAll()
                    .Include(p => p.Trip)
                    .Include(p => p.Earning)
                    .Where(p => p.EarningId != null
                        && p.Trip != null
                        && p.Trip.Status == TripStatus.Completed
                        && p.Trip.EndedAt.HasValue
                        && p.Trip.EndedAt.Value >= fromDate
                        && p.Trip.EndedAt.Value <= toDate
                        && p.Earning != null);

                // Calculate app profit: Payment.Amount - Earning.Amount
                var totalAppProfit = await payments.SumAsync(p => p.Amount - p.Earning!.Amount);
                return totalAppProfit;
            });

            _cache.Set(cacheKey, amount, CACHE_EXPIRATION);
            _logger.LogDebug("Cached app profits: {Amount} for period {FromDate} to {ToDate}", amount, fromDate, toDate);

            return amount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting app profits");
            return 0;
        }
    }
}

