namespace AdminLine.Common.DTOs;

public class DriverSortDto
{
    public DriverSortField SortBy { get; set; } = DriverSortField.Name;
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}

