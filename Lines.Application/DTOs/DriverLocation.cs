namespace Lines.Application.DTOs;

public class DriverLocation
{
    public Guid DriverId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<string> ConnectionIds { get; set; } = new();
    public double DistanceKm { get; set; }
}