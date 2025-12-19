namespace Lines.Application.Features.Drivers.GetDriverRatingsSummary.DTOs;

public class DriverRatingsSummaryDto
{
    public int TotalRates { get; set; }
    public double AverageRating { get; set; }
    public List<RatingRangeDto> Rates { get; set; } = new List<RatingRangeDto>();
}

public class RatingRangeDto
{
    public string Range { get; set; } = string.Empty; // "moreThan4.5", "4to4.5", etc.
    public int Count { get; set; }
}
