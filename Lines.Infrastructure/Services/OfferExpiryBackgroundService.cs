using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lines.Infrastructure.Services;

public class OfferExpiryBackgroundService : BackgroundService
{
    private readonly ILogger<OfferExpiryBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

    public OfferExpiryBackgroundService(
        ILogger<OfferExpiryBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Offer Expiry Background Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DeactivateExpiredOffersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deactivating expired offers.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Offer Expiry Background Service is stopping.");
    }

    private async Task DeactivateExpiredOffersAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var activationRepository = scope.ServiceProvider.GetRequiredService<IRepository<DriverOfferActivation>>();

        var now = DateTime.UtcNow;

        // Find all active offers that have expired
        var expiredActivations = await activationRepository
            .Get(a => a.IsActive && a.ExpirationDate <= now)
            .ToListAsync(cancellationToken);

        if (expiredActivations.Any())
        {
            _logger.LogInformation($"Found {expiredActivations.Count} expired offer activations to deactivate.");

            foreach (var activation in expiredActivations)
            {
                activation.Deactivate();
                await activationRepository.UpdateAsync(activation, cancellationToken);

                _logger.LogInformation(
                    "Deactivated offer activation {ActivationId} for driver {DriverId}. " +
                    "Offer expired on {ExpirationDate}. Service fee will revert to default for future trips.",
                    activation.Id, activation.DriverId, activation.ExpirationDate);
            }

            await activationRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Successfully deactivated {expiredActivations.Count} expired offers.");
        }
        else
        {
            _logger.LogDebug("No expired offer activations found.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Offer Expiry Background Service is stopping gracefully.");
        await base.StopAsync(cancellationToken);
    }
}

