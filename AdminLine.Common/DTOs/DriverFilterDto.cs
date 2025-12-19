namespace AdminLine.Common.DTOs;

public class DriverFilterDto
{
    public string? SearchTerm { get; set; }
    public DriverStatus? Status { get; set; }
    public int? MinTrips { get; set; }
    public int? MaxTrips { get; set; }
    public decimal? MinEarnings { get; set; }
    public decimal? MaxEarnings { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }
}

