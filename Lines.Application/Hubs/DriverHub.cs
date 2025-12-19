using Lines.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Lines.Application.Hubs;
public class DriverHub : Hub
{
    private readonly IDriverConnectionService _driverConnectionService;

    public DriverHub(IDriverConnectionService driverConnectionService)
    {
        _driverConnectionService = driverConnectionService;
    }

    public async Task JoinAsAvailableDriver(Guid driverId, double latitude, double longitude)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "AvailableDrivers");
        await _driverConnectionService.AddDriverConnectionAsync(driverId, Context.ConnectionId);
        await _driverConnectionService.UpdateDriverLocationAsync(driverId, latitude, longitude);

        // Notify that driver is online
        await Clients.Caller.SendAsync("DriverStatusUpdated", "Available");
    }

    public async Task UpdateDriverLocation(Guid driverId, double latitude, double longitude)
    {
        await _driverConnectionService.UpdateDriverLocationAsync(driverId, latitude, longitude);

        // Optionally broadcast location to admin/monitoring systems
        await Clients.Group("AdminMonitoring").SendAsync("DriverLocationUpdated", driverId, latitude, longitude);
    }

    public async Task LeaveAvailableDrivers(Guid driverId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AvailableDrivers");
        await _driverConnectionService.RemoveDriverConnectionAsync(driverId, Context.ConnectionId);

        // Notify that driver is offline
        await Clients.Caller.SendAsync("DriverStatusUpdated", "Offline");
    }

    public async Task JoinRiderGroup(Guid riderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Rider_{riderId}");
    }
    public async Task JoinDriverGroup(Guid driverId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Driver_{driverId}");
    }
    public async Task JoinAdminMonitoring()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "AdminMonitoring");
    }

    public async Task GetNearbyDrivers(double latitude, double longitude, double radiusKm = 10.0)
    {
        var nearbyDrivers = await _driverConnectionService.GetNearbyDriversAsync(latitude, longitude, radiusKm);
        await Clients.Caller.SendAsync("NearbyDriversResponse", nearbyDrivers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _driverConnectionService.RemoveConnectionAsync(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}