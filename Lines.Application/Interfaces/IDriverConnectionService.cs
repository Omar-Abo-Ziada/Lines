using Lines.Application.DTOs;

namespace Lines.Application.Interfaces;

public interface IDriverConnectionService
{
    Task AddDriverConnectionAsync(Guid driverId, string connectionId);
    Task RemoveDriverConnectionAsync(Guid driverId, string connectionId);
    Task RemoveConnectionAsync(string connectionId);
    Task<List<string>> GetDriverConnectionsAsync(Guid driverId);
    Task UpdateDriverLocationAsync(Guid driverId, double latitude, double longitude);
    Task<List<DriverLocation>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm = 10.0);
    List<Guid> GetDriverIdsFromConnections(List<string> connectionIds);
    Guid GetDriverIdFromConnection(string connectionId);
}