using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Services;

/// <summary>
/// Service that calculates the applicable service fee for a driver,
/// considering active promotional offers.
/// </summary>
public class ServiceFeeService : IServiceFeeService
{
    private readonly IRepository<DriverOfferActivation> _activationRepository;
    private readonly ILogger<ServiceFeeService> _logger;
    
    // Default service fee percentage when no active offer exists
    private const decimal DEFAULT_SERVICE_FEE_PERCENT = 15.0m;

    public ServiceFeeService(
        IRepository<DriverOfferActivation> activationRepository,
        ILogger<ServiceFeeService> logger)
    {
        _activationRepository = activationRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets the applicable service fee percentage for a driver.
    /// Prioritizes active offers, falls back to default if none exist.
    /// </summary>
    public async Task<decimal> GetApplicableServiceFeePercentAsync(
        Guid driverId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var now = DateTime.UtcNow;

            // Check for active offer with service fee discount
            var activeOffer = await _activationRepository
                .Get(a => a.DriverId == driverId 
                       && a.IsActive 
                       && a.ExpirationDate > now)
                .Include(a => a.Offer)
                .OrderByDescending(a => a.ActivationDate) // Get most recent if multiple
                .FirstOrDefaultAsync(cancellationToken);

            if (activeOffer != null && activeOffer.Offer != null)
            {
                _logger.LogInformation(
                    "Active offer found for driver {DriverId}. " +
                    "Applying reduced service fee: {ServiceFeePercent}% (Offer: {OfferTitle})",
                    driverId, activeOffer.Offer.ServiceFeePercent, activeOffer.Offer.Title);

                return activeOffer.Offer.ServiceFeePercent;
            }

            // No active offer - use default service fee
            _logger.LogDebug(
                "No active offer for driver {DriverId}. Using default service fee: {DefaultPercent}%",
                driverId, DEFAULT_SERVICE_FEE_PERCENT);

            return DEFAULT_SERVICE_FEE_PERCENT;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving service fee for driver {DriverId}. Falling back to default: {DefaultPercent}%",
                driverId, DEFAULT_SERVICE_FEE_PERCENT);

            // Fallback to default on error to prevent blocking payment
            return DEFAULT_SERVICE_FEE_PERCENT;
        }
    }
}

