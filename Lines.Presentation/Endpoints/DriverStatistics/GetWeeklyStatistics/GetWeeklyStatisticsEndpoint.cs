using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Features.DriverStatistics.GetWeeklyStatistics.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetWeeklyStatistics;

public class GetWeeklyStatisticsEndpoint : BaseController<GetWeeklyStatisticsRequest, GetWeeklyStatisticsResponse>
{
    private readonly BaseControllerParams<GetWeeklyStatisticsRequest> _dependencyCollection;

    public GetWeeklyStatisticsEndpoint(BaseControllerParams<GetWeeklyStatisticsRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("driver-statistics/weekly/{driverId}")]
    [Authorize]
    public async Task<ApiResponse<GetWeeklyStatisticsResponse>> GetWeeklyStatistics(
        Guid driverId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var request = new GetWeeklyStatisticsRequest(driverId, from, to);

        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new GetWeeklyStatisticsOrchestrator(driverId, from, to));

        return HandleResult<WeeklyStatisticsDto, GetWeeklyStatisticsResponse>(result);
    }
}


