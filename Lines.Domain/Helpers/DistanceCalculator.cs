using Lines.Domain.Value_Objects;

namespace Lines.Domain.Helpers;

public static class DistanceCalculator
{
    public static float CalculateDistance(Location a, Location b)
    {
        if (a == null || b == null)
            return 0;

        var dx = a.Latitude - b.Latitude;
        var dy = a.Longitude - b.Longitude;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public static float CalculateDistance(double late1, double late2, double long1, double long2)
    {
        var dx = late1 - late2;
        var dy = long1 - long2;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    // public static float CalculateTotalDistance(Location startLocation, IEnumerable<EndTripLocation> endLocations)
    // {
    //     if (startLocation == null || endLocations == null || !endLocations.Any())
    //         return 0;
    //
    //     var orderedLocations = endLocations
    //         .OrderBy(e => e.Order)
    //         .Select(e => e.Location)
    //         .Where(l => l != null)
    //         .ToList();
    //
    //     float totalDistance = 0;
    //     var currentLocation = startLocation;
    //
    //     foreach (var nextLocation in orderedLocations)
    //     {
    //         totalDistance += CalculateDistance(currentLocation, nextLocation);
    //         currentLocation = nextLocation;
    //     }
    //
    //     return totalDistance;
    // }
}
