using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Features.DriverStatistics.GetDailyStatistics.Queries;

namespace Lines.Application.Features.DriverStatistics.GetDailyStatistics.Orchestrator;

public record GetDailyStatisticsOrchestrator(Guid DriverId) : IRequest<Result<DailyStatisticsDto>>;

public class GetDailyStatisticsOrchestratorHandler : RequestHandlerBase<GetDailyStatisticsOrchestrator, Result<DailyStatisticsDto>>
{
    public GetDailyStatisticsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<DailyStatisticsDto>> Handle(GetDailyStatisticsOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDailyStatisticsQuery(request.DriverId), cancellationToken);
        return result;
    }
}


