namespace AdminLine.Service.IService;

public interface IFinanceService
{
    Task<decimal> GetDriversProfitsAsync(DateTime fromDate, DateTime toDate);
    Task<decimal> GetAppProfitsAsync(DateTime fromDate, DateTime toDate);
}

