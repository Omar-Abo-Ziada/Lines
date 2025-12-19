using AdminLine.Common.DTOs;
using AdminLine.Common.Helper;

namespace AdminLine.Service.IService;

public interface IDriverAdminService
{
    Task<PagingDto<DriverListDto>> GetDriversAsync(
        DriverFilterDto filter,
        DriverSortDto sort,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<DriverDetailDto?> GetDriverByIdAsync(Guid driverId, CancellationToken cancellationToken = default);

    Task<bool> UpdateDriverStatusAsync(UpdateDriverStatusDto request, CancellationToken cancellationToken = default);

    Task<byte[]> ExportDriversAsync(
        DriverFilterDto filter,
        DriverSortDto sort,
        CancellationToken cancellationToken = default);

    Task<DriverProfileHeaderDto?> GetDriverProfileHeaderAsync(Guid driverId, CancellationToken cancellationToken = default);
    Task<DriverPersonalInformationDto?> GetDriverPersonalInformationAsync(Guid driverId, CancellationToken cancellationToken = default);
    Task<DriverDashboardDto?> GetDriverDashboardAsync(Guid driverId, CancellationToken cancellationToken = default);
}

