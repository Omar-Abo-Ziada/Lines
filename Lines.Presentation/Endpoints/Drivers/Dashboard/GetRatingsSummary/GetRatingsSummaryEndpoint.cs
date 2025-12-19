using Lines.Application.Features.Drivers.GetDriverRatingsSummary.DTOs;
using Lines.Application.Features.Drivers.GetDriverRatingsSummary.Orchestrators;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Dashboard.GetRatingsSummary;

public class GetRatingsSummaryEndpoint : BaseController<GetRatingsSummaryRequest, GetRatingsSummaryResponse>
{
    private readonly BaseControllerParams<GetRatingsSummaryRequest> _dependencyCollection;
    
    public GetRatingsSummaryEndpoint(BaseControllerParams<GetRatingsSummaryRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/ratings-summary")]
    public async Task<ApiResponse<GetRatingsSummaryResponse>> GetRatingsSummary()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<DriverRatingsSummaryDto, GetRatingsSummaryResponse>(
                Result<DriverRatingsSummaryDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        // Get driver ID from user ID using ApplicationUserService
        var applicationUserService = HttpContext.RequestServices.GetRequiredService<IApplicationUserService>();
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(userId);
        
        if (userDriverIds?.DriverId == null)
        {
            return HandleResult<DriverRatingsSummaryDto, GetRatingsSummaryResponse>(
                Result<DriverRatingsSummaryDto>.Failure(new Lines.Application.Shared.Error("NOT_DRIVER", "User is not a driver", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetDriverRatingsSummaryOrchestrator(userId));
        return HandleResult<DriverRatingsSummaryDto, GetRatingsSummaryResponse>(result);
    }
}
