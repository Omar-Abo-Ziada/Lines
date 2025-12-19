using AdminLine.Framework.UoW;
using AdminLine.Service.IService;
using Lines.Application.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AdminLine.Service.Service;

public class DriverService : IDriverService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private readonly IDriverConnectionService _driverConnectionService;
    private readonly ILogger<DriverService> _logger;

    private const string CACHE_KEY_ALL_DRIVERS = "dashboard:all_drivers";
    private const string CACHE_KEY_ONLINE_DRIVERS = "dashboard:online_drivers";
    private const string CACHE_KEY_ON_TRIP = "dashboard:on_trip";
    private static readonly TimeSpan CACHE_EXPIRATION_ALL_DRIVERS = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan CACHE_EXPIRATION_ONLINE = TimeSpan.FromSeconds(45);
    private static readonly TimeSpan CACHE_EXPIRATION_ON_TRIP = TimeSpan.FromSeconds(45);

    public DriverService(
        IUnitOfWork unitOfWork,
        IMemoryCache cache,
        IDriverConnectionService driverConnectionService,
        ILogger<DriverService> logger)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _driverConnectionService = driverConnectionService;
        _logger = logger;
    }

    public async Task<int> GetAllDriversCountAsync()
    {
        try
        {
            if (_cache.TryGetValue(CACHE_KEY_ALL_DRIVERS, out int cachedCount))
            {
                _logger.LogDebug("Cache hit for all drivers count");
                return cachedCount;
            }

            var repository = _unitOfWork.GetRepository<Driver>();
            var count = await Task.Run(() =>
            {
                return repository.GetAll(d => !d.IsDeleted).Count();
            });

            _cache.Set(CACHE_KEY_ALL_DRIVERS, count, CACHE_EXPIRATION_ALL_DRIVERS);
            _logger.LogDebug("Cached all drivers count: {Count}", count);

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all drivers count");
            return 0;
        }
    }

    public async Task<int> GetOnlineDriversCountAsync()
    {
        try
        {
            if (_cache.TryGetValue(CACHE_KEY_ONLINE_DRIVERS, out int cachedCount))
            {
                _logger.LogDebug("Cache hit for online drivers count");
                return cachedCount;
            }

            // Get online drivers from SignalR connections cache
            var count = await Task.Run(() =>
            {
                // Access the cache directly using the same key pattern as DriverConnectionService
                const string DRIVER_CONNECTIONS_KEY = "driver_connections";
                if (_cache.TryGetValue(DRIVER_CONNECTIONS_KEY, out Dictionary<Guid, List<string>>? connections))
                {
                    return connections?.Count ?? 0;
                }
                return 0;
            });

            _cache.Set(CACHE_KEY_ONLINE_DRIVERS, count, CACHE_EXPIRATION_ONLINE);
            _logger.LogDebug("Cached online drivers count: {Count}", count);

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting online drivers count");
            return 0;
        }
    }

    public async Task<int> GetOnTripDriversCountAsync()
    {
        try
        {
            if (_cache.TryGetValue(CACHE_KEY_ON_TRIP, out int cachedCount))
            {
                _logger.LogDebug("Cache hit for on trip drivers count");
                return cachedCount;
            }

            var repository = _unitOfWork.GetRepository<Trip>();
            var count = await Task.Run(() =>
            {
                return repository.GetAll(t => t.Status == TripStatus.InProgress)
                    .Select(t => t.DriverId)
                    .Distinct()
                    .Count();
            });

            _cache.Set(CACHE_KEY_ON_TRIP, count, CACHE_EXPIRATION_ON_TRIP);
            _logger.LogDebug("Cached on trip drivers count: {Count}", count);

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting on trip drivers count");
            return 0;
        }
    }
}

