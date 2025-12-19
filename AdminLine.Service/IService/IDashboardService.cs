using AdminLine.Common.DTOs;

namespace AdminLine.Service.IService;

public interface IDashboardService
{
    Task<DashboardOverviewDto> GetDashboardOverviewAsync();
}

