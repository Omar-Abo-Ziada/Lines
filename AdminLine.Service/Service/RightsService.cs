using AdminLine.Framework.UoW;
using AdminLine.Service.IService;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AdminLine.Service.Service;

public class RightsService : IRightsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RightsService> _logger;

    private static readonly TimeSpan CACHE_EXPIRATION = TimeSpan.FromHours(1);

    public RightsService(
        IUnitOfWork unitOfWork,
        IMemoryCache cache,
        ILogger<RightsService> logger)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<decimal> GetDriversRightsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var cacheKey = $"dashboard:drivers_rights:{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";
            
            if (_cache.TryGetValue(cacheKey, out decimal cachedAmount))
            {
                _logger.LogDebug("Cache hit for drivers rights");
                return cachedAmount;
            }

            var earningRepository = _unitOfWork.GetRepository<Earning>();

            var amount = await Task.Run(async () =>
            {
                // Get all unpaid earnings (IsPaid = false) in the date range
                var earnings = earningRepository.GetAll()
                    .Where(e => !e.IsPaid
                        && e.EarnedAt >= fromDate
                        && e.EarnedAt <= toDate);

                return await earnings.SumAsync(e => e.Amount);
            });

            _cache.Set(cacheKey, amount, CACHE_EXPIRATION);
            _logger.LogDebug("Cached drivers rights: {Amount} for period {FromDate} to {ToDate}", amount, fromDate, toDate);

            return amount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drivers rights");
            return 0;
        }
    }

    public async Task<decimal> GetAppRightsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var cacheKey = $"dashboard:app_rights:{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}";
            
            if (_cache.TryGetValue(cacheKey, out decimal cachedAmount))
            {
                _logger.LogDebug("Cache hit for app rights");
                return cachedAmount;
            }

            var paymentRepository = _unitOfWork.GetRepository<Payment>();
            var earningRepository = _unitOfWork.GetRepository<Earning>();

            var amount = await Task.Run(async () =>
            {
                // Get all payments with unpaid earnings (IsPaid = false) in the date range
                // Note: We query from Earning side since that's where IsPaid flag is
                var earningRepository = _unitOfWork.GetRepository<Earning>();
                var unpaidEarnings = earningRepository.GetAll()
                    .Include(e => e.Payment)
                    .Where(e => !e.IsPaid
                        && e.EarnedAt >= fromDate
                        && e.EarnedAt <= toDate
                        && e.Payment != null);

                // Calculate app rights: Payment.Amount - Earning.Amount (for unpaid earnings)
                var totalAppRights = await unpaidEarnings.SumAsync(e => e.Payment!.Amount - e.Amount);
                return totalAppRights;
            });

            _cache.Set(cacheKey, amount, CACHE_EXPIRATION);
            _logger.LogDebug("Cached app rights: {Amount} for period {FromDate} to {ToDate}", amount, fromDate, toDate);

            return amount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting app rights");
            return 0;
        }
    }
}

