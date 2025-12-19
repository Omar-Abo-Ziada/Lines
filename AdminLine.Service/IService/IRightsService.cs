namespace AdminLine.Service.IService;

public interface IRightsService
{
    Task<decimal> GetDriversRightsAsync(DateTime fromDate, DateTime toDate);
    Task<decimal> GetAppRightsAsync(DateTime fromDate, DateTime toDate);
}

