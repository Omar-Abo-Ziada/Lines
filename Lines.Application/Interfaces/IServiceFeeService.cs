namespace Lines.Application.Interfaces;

/// <summary>
/// Service to determine the applicable service fee percentage for a driver
/// based on active promotional offers or default configuration.
/// </summary>
public interface IServiceFeeService
{
    /// <summary>
    /// Gets the applicable service fee percentage for a driver.
    /// Checks for active offers first, then falls back to default.
    /// </summary>
    /// <param name="driverId">The driver's unique identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Service fee percentage (e.g., 15.0 for 15%)</returns>
    Task<decimal> GetApplicableServiceFeePercentAsync(Guid driverId, CancellationToken cancellationToken = default);
}

