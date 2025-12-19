using Lines.Application.Features.Drivers.GetDriverTrips.DTOs;
using Lines.Application.Features.Drivers.GetDriverTrips.Orchestrators;
using Lines.Application.Interfaces;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Dashboard.GetTrips;

public class GetDriverTripsEndpoint : BaseController<GetDriverTripsRequest, GetDriverTripsResponse>
{
    private readonly BaseControllerParams<GetDriverTripsRequest> _dependencyCollection;
    
    public GetDriverTripsEndpoint(BaseControllerParams<GetDriverTripsRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/trips")]
    public async Task<ApiResponse<GetDriverTripsResponse>> GetTrips([FromQuery] GetDriverTripsRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<PagingDto<DriverTripDto>, GetDriverTripsResponse>(
                Result<PagingDto<DriverTripDto>>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        // Get driver ID from user ID using ApplicationUserService
        var applicationUserService = HttpContext.RequestServices.GetRequiredService<IApplicationUserService>();
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(userId);
        
        if (userDriverIds?.DriverId == null)
        {
            return HandleResult<PagingDto<DriverTripDto>, GetDriverTripsResponse>(
                Result<PagingDto<DriverTripDto>>.Failure(new Lines.Application.Shared.Error("NOT_DRIVER", "User is not a driver", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetDriverTripsOrchestrator(
            userDriverIds.DriverId.Value,
            request.TripStatus,
            request.DateRangeStart,
            request.DateRangeEnd,
            request.PaymentStatus,
            request.PageNumber,
            request.PageSize
        ));

        return HandleResult<PagingDto<DriverTripDto>, GetDriverTripsResponse>(result);
    }
}
