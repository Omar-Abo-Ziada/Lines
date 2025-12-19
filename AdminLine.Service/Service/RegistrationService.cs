using AdminLine.Framework.UoW;
using AdminLine.Service.IService;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AdminLine.Service.Service;

public class RegistrationService : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RegistrationService> _logger;

    private const string CACHE_KEY_PENDING_REGISTRATIONS = "dashboard:pending_registrations";
    private static readonly TimeSpan CACHE_EXPIRATION = TimeSpan.FromMinutes(5);

    public RegistrationService(
        IUnitOfWork unitOfWork,
        IMemoryCache cache,
        ILogger<RegistrationService> logger)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<int> GetPendingRegistrationsCountAsync()
    {
        try
        {
            if (_cache.TryGetValue(CACHE_KEY_PENDING_REGISTRATIONS, out int cachedCount))
            {
                _logger.LogDebug("Cache hit for pending registrations count");
                return cachedCount;
            }

            var repository = _unitOfWork.GetRepository<DriverRegistration>();
            var count = await Task.Run(() =>
            {
                return repository.GetAll(r => r.Status == RegistrationStatus.PendingReview).Count();
            });

            _cache.Set(CACHE_KEY_PENDING_REGISTRATIONS, count, CACHE_EXPIRATION);
            _logger.LogDebug("Cached pending registrations count: {Count}", count);

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending registrations count");
            return 0;
        }
    }
}

