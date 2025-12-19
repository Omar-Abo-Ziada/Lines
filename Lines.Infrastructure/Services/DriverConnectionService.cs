using Lines.Application.DTOs;
using Lines.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Lines.Infrastructure.Services;
public class DriverConnectionService : IDriverConnectionService
{
    private readonly IMemoryCache _cache;
    private readonly string DRIVER_CONNECTIONS_KEY = "driver_connections";
    private readonly string DRIVER_LOCATIONS_KEY = "driver_locations";

    public DriverConnectionService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task AddDriverConnectionAsync(Guid driverId, string connectionId)   ///TODO: get id from authenticated user instead of getting it from front
    {
        var connections = GetDriverConnections();
        
        if (!connections.ContainsKey(driverId))
            connections[driverId] = new List<string>();
        
        if (!connections[driverId].Contains(connectionId))
            connections[driverId].Add(connectionId);
        
        _cache.Set(DRIVER_CONNECTIONS_KEY, connections, TimeSpan.FromHours(24));
        await Task.CompletedTask;
    }

    public async Task RemoveDriverConnectionAsync(Guid driverId, string connectionId)
    {
        var connections = GetDriverConnections();
        
        if (connections.ContainsKey(driverId))
        {
            connections[driverId].Remove(connectionId);
            if (connections[driverId].Count == 0)
            {
                connections.Remove(driverId);
                // Also remove location when driver disconnects completely
                var locations = GetDriverLocations();
                locations.Remove(driverId);
                _cache.Set(DRIVER_LOCATIONS_KEY, locations, TimeSpan.FromHours(24));
            }
        }
        
        _cache.Set(DRIVER_CONNECTIONS_KEY, connections, TimeSpan.FromHours(24));
        await Task.CompletedTask;
    }

    public async Task RemoveConnectionAsync(string connectionId)
    {
        var connections = GetDriverConnections();
        
        foreach (var kvp in connections.ToList())
        {
            kvp.Value.Remove(connectionId);
            if (kvp.Value.Count == 0)
            {
                connections.Remove(kvp.Key);
                // Also remove location when driver disconnects completely
                var locations = GetDriverLocations();
                locations.Remove(kvp.Key);
                _cache.Set(DRIVER_LOCATIONS_KEY, locations, TimeSpan.FromHours(24));
            }
        }
        
        _cache.Set(DRIVER_CONNECTIONS_KEY, connections, TimeSpan.FromHours(24));
        await Task.CompletedTask;
    }

    public async Task<List<string>> GetDriverConnectionsAsync(Guid driverId)
    {
        var connections = GetDriverConnections();
        return connections.ContainsKey(driverId) ? connections[driverId] : new List<string>();
    }

    public async Task UpdateDriverLocationAsync(Guid driverId, double latitude, double longitude)
    {
        var locations = GetDriverLocations();
        var connections = GetDriverConnections();
        
        // Only update location if driver has active connections
        if (connections.ContainsKey(driverId) && connections[driverId].Count > 0)
        {
            locations[driverId] = new DriverLocation
            {
                DriverId = driverId,
                Latitude = latitude,
                Longitude = longitude,
                LastUpdated = DateTime.UtcNow,
                ConnectionIds = connections[driverId]
            };
            
            _cache.Set(DRIVER_LOCATIONS_KEY, locations, TimeSpan.FromHours(24));
        }
        
        await Task.CompletedTask;
    }



    public async Task<List<DriverLocation>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm = 10.0)
    {
        var locations = GetDriverLocations();
        var nearbyDrivers = new List<DriverLocation>();
        
        foreach (var kvp in locations)
        {
            var driver = kvp.Value;
            var distance = CalculateDistance(latitude, longitude, driver.Latitude, driver.Longitude); 
            
            if (distance <= radiusKm)
            {
                driver.DistanceKm = Math.Round(distance, 2);
                nearbyDrivers.Add(driver);
            }
        }
        
        // Sort by distance (closest first)
        nearbyDrivers.Sort((a, b) => a.DistanceKm.CompareTo(b.DistanceKm));
        
        await Task.CompletedTask;
        return nearbyDrivers;
    }

    private Dictionary<Guid, List<string>> GetDriverConnections()
    {
        return _cache.GetOrCreate(DRIVER_CONNECTIONS_KEY, _ => new Dictionary<Guid, List<string>>())!;
    }

    private Dictionary<Guid, DriverLocation> GetDriverLocations()
    {
        return _cache.GetOrCreate(DRIVER_LOCATIONS_KEY, _ => new Dictionary<Guid, DriverLocation>())!;
    }

    private double CalculateDistance(double lat1, double lng1, double lat2, double lng2)
    {
        const double earthRadius = 6371; // Earth's radius in kilometers
        
        var dLat = ToRadians(lat2 - lat1);
        var dLng = ToRadians(lng2 - lng1);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return earthRadius * c;
    }

    private double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    #region FCM Firebase

    public Guid GetDriverIdFromConnection(string connectionId)
    {
        var allConnections = GetDriverConnections();

        foreach (var kv in allConnections)
        {
            if (kv.Value.Contains(connectionId))
                return kv.Key;
        }

        return Guid.Empty;
    }


    public List<Guid> GetDriverIdsFromConnections(List<string> connectionIds)
    {
        var result = new List<Guid>();
        var allConnections = GetDriverConnections();

        foreach (var kv in allConnections)
        {
            if (kv.Value.Any(cid => connectionIds.Contains(cid)))
                result.Add(kv.Key);
        }

        return result;
    }

    #endregion
}