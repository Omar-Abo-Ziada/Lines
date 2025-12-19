using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Features.DriverStatistics.GetDailyStatistics.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetDailyStatistics;

public class GetDailyStatisticsEndpoint : BaseController<GetDailyStatisticsRequest, GetDailyStatisticsResponse>
{
    private readonly BaseControllerParams<GetDailyStatisticsRequest> _dependencyCollection;

    public GetDailyStatisticsEndpoint(BaseControllerParams<GetDailyStatisticsRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("driver-statistics/daily/{driverId}")]
    [Authorize]
    public async Task<ApiResponse<GetDailyStatisticsResponse>> GetDailyStatistics(Guid driverId)
    {
        var request = new GetDailyStatisticsRequest(driverId);

        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new GetDailyStatisticsOrchestrator(driverId));

        return HandleResult<DailyStatisticsDto, GetDailyStatisticsResponse>(result);
    }
}


