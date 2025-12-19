namespace AdminLine.Common.DTOs;

public class KpiCardDto
{
    public decimal Value { get; set; }
    public decimal? PercentageChange { get; set; }
    public string ComparisonDescription { get; set; } = string.Empty;
    public ChangeType ChangeType { get; set; }
}

