namespace AdminLine.Common.DTOs;

public class PercentageChangeDto
{
    public decimal CurrentValue { get; set; }
    public decimal PreviousValue { get; set; }
    public decimal? PercentageChange { get; set; }
    public string Description { get; set; } = string.Empty;
    public ChangeType ChangeType { get; set; }
}

public enum ChangeType
{
    Increase,
    Decrease,
    NoChange
}

