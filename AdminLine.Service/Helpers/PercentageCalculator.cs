using AdminLine.Common.DTOs;

namespace AdminLine.Service.Helpers;

public static class PercentageCalculator
{
    public static PercentageChangeDto CalculatePercentageChange(
        decimal currentValue,
        decimal previousValue,
        string periodDescription)
    {
        if (previousValue == 0)
        {
            // If previous value is 0, we can't calculate percentage change
            // Return null for percentage change
            return new PercentageChangeDto
            {
                CurrentValue = currentValue,
                PreviousValue = previousValue,
                PercentageChange = null,
                Description = currentValue > 0 ? "New" : "No change",
                ChangeType = currentValue > 0 ? ChangeType.Increase : ChangeType.NoChange
            };
        }

        var percentageChange = ((currentValue - previousValue) / previousValue) * 100;
        var roundedPercentage = Math.Round(percentageChange, 2);

        var changeType = roundedPercentage switch
        {
            > 0 => ChangeType.Increase,
            < 0 => ChangeType.Decrease,
            _ => ChangeType.NoChange
        };

        var sign = roundedPercentage > 0 ? "+" : "";
        var description = $"{sign}{roundedPercentage}% {periodDescription}";

        return new PercentageChangeDto
        {
            CurrentValue = currentValue,
            PreviousValue = previousValue,
            PercentageChange = roundedPercentage,
            Description = description,
            ChangeType = changeType
        };
    }
}

