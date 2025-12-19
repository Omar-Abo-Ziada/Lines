using AdminLine.Common.DTOs;
using AdminLine.Common.Helper;
using AdminLine.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AdminLine.Controllers;

[ApiController]
[Route("api/admin/drivers")]
public class DriversAdminController : ControllerBase
{
    private readonly IDriverAdminService _driverAdminService;
    private readonly ILogger<DriversAdminController> _logger;

    public DriversAdminController(
        IDriverAdminService driverAdminService,
        ILogger<DriversAdminController> logger)
    {
        _driverAdminService = driverAdminService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagingDto<DriverListDto>>>> GetDrivers(
        [FromQuery] string? searchTerm = null,
        [FromQuery] DriverStatus? status = null,
        [FromQuery] int? minTrips = null,
        [FromQuery] int? maxTrips = null,
        [FromQuery] decimal? minEarnings = null,
        [FromQuery] decimal? maxEarnings = null,
        [FromQuery] double? minRating = null,
        [FromQuery] double? maxRating = null,
        [FromQuery] DriverSortField sortBy = DriverSortField.Name,
        [FromQuery] SortDirection sortDirection = SortDirection.Ascending,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var filter = new DriverFilterDto
            {
                SearchTerm = searchTerm,
                Status = status,
                MinTrips = minTrips,
                MaxTrips = maxTrips,
                MinEarnings = minEarnings,
                MaxEarnings = maxEarnings,
                MinRating = minRating,
                MaxRating = maxRating
            };

            var sort = new DriverSortDto
            {
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            var result = await _driverAdminService.GetDriversAsync(filter, sort, pageNumber, pageSize);
            return ApiResponse<PagingDto<DriverListDto>>.SuccessResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drivers list");
            return ApiResponse<PagingDto<DriverListDto>>.ErrorResponse(
                Error.Create("DRIVERS_ERROR", "Failed to retrieve drivers list"),
                500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DriverDetailDto>>> GetDriverById(Guid id)
    {
        try
        {
            var driver = await _driverAdminService.GetDriverByIdAsync(id);

            if (driver == null)
            {
                return ApiResponse<DriverDetailDto>.ErrorResponse(
                    Error.Create("DRIVER_NOT_FOUND", "Driver not found"),
                    404);
            }

            return ApiResponse<DriverDetailDto>.SuccessResponse(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver by ID: {DriverId}", id);
            return ApiResponse<DriverDetailDto>.ErrorResponse(
                Error.Create("DRIVER_ERROR", "Failed to retrieve driver details"),
                500);
        }
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateDriverStatus(
        Guid id,
        [FromBody] UpdateDriverStatusDto request)
    {
        try
        {
            if (request.DriverId != id)
            {
                return ApiResponse<bool>.ErrorResponse(
                    Error.Create("VALIDATION_ERROR", "Driver ID in URL must match request body"),
                    400);
            }

            var result = await _driverAdminService.UpdateDriverStatusAsync(request);

            if (!result)
            {
                return ApiResponse<bool>.ErrorResponse(
                    Error.Create("DRIVER_NOT_FOUND", "Driver not found or could not be updated"),
                    404);
            }

            return ApiResponse<bool>.SuccessResponse(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating driver status: {DriverId}", id);
            return ApiResponse<bool>.ErrorResponse(
                Error.Create("DRIVER_ERROR", "Failed to update driver status"),
                500);
        }
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportDrivers(
        [FromQuery] string? searchTerm = null,
        [FromQuery] DriverStatus? status = null,
        [FromQuery] int? minTrips = null,
        [FromQuery] int? maxTrips = null,
        [FromQuery] decimal? minEarnings = null,
        [FromQuery] decimal? maxEarnings = null,
        [FromQuery] double? minRating = null,
        [FromQuery] double? maxRating = null,
        [FromQuery] DriverSortField sortBy = DriverSortField.Name,
        [FromQuery] SortDirection sortDirection = SortDirection.Ascending)
    {
        try
        {
            var filter = new DriverFilterDto
            {
                SearchTerm = searchTerm,
                Status = status,
                MinTrips = minTrips,
                MaxTrips = maxTrips,
                MinEarnings = minEarnings,
                MaxEarnings = maxEarnings,
                MinRating = minRating,
                MaxRating = maxRating
            };

            var sort = new DriverSortDto
            {
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            var csvData = await _driverAdminService.ExportDriversAsync(filter, sort);

            var fileName = $"drivers_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            return File(csvData, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting drivers");
            return StatusCode(500, "Failed to export drivers");
        }
    }

    [HttpGet("{id}/profile/header")]
    public async Task<ActionResult<ApiResponse<DriverProfileHeaderDto>>> GetDriverProfileHeader(Guid id)
    {
        try
        {
            var driver = await _driverAdminService.GetDriverProfileHeaderAsync(id);

            if (driver == null)
            {
                return ApiResponse<DriverProfileHeaderDto>.ErrorResponse(
                    Error.Create("DRIVER_NOT_FOUND", "Driver not found"),
                    404);
            }

            return ApiResponse<DriverProfileHeaderDto>.SuccessResponse(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver profile header: {DriverId}", id);
            return ApiResponse<DriverProfileHeaderDto>.ErrorResponse(
                Error.Create("DRIVER_ERROR", "Failed to retrieve driver profile header"),
                500);
        }
    }

    [HttpGet("{id}/profile/personal-information")]
    public async Task<ActionResult<ApiResponse<DriverPersonalInformationDto>>> GetDriverPersonalInformation(Guid id)
    {
        try
        {
            var driver = await _driverAdminService.GetDriverPersonalInformationAsync(id);

            if (driver == null)
            {
                return ApiResponse<DriverPersonalInformationDto>.ErrorResponse(
                    Error.Create("DRIVER_NOT_FOUND", "Driver not found"),
                    404);
            }

            return ApiResponse<DriverPersonalInformationDto>.SuccessResponse(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver personal information: {DriverId}", id);
            return ApiResponse<DriverPersonalInformationDto>.ErrorResponse(
                Error.Create("DRIVER_ERROR", "Failed to retrieve driver personal information"),
                500);
        }
    }

    [HttpGet("{id}/dashboard")]
    public async Task<ActionResult<ApiResponse<DriverDashboardDto>>> GetDriverDashboard(Guid id)
    {
        try
        {
            var dashboard = await _driverAdminService.GetDriverDashboardAsync(id);

            if (dashboard == null)
            {
                return ApiResponse<DriverDashboardDto>.ErrorResponse(
                    Error.Create("DRIVER_NOT_FOUND", "Driver not found"),
                    404);
            }

            return ApiResponse<DriverDashboardDto>.SuccessResponse(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver dashboard: {DriverId}", id);
            return ApiResponse<DriverDashboardDto>.ErrorResponse(
                Error.Create("DRIVER_ERROR", "Failed to retrieve driver dashboard"),
                500);
        }
    }
}

