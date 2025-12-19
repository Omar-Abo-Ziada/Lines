using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Features.DriverStatistics.GetWeeklyStatistics.Queries;

namespace Lines.Application.Features.DriverStatistics.GetWeeklyStatistics.Orchestrator;

public record GetWeeklyStatisticsOrchestrator(Guid DriverId, DateTime? FromDate, DateTime? ToDate) : IRequest<Result<WeeklyStatisticsDto>>;

public class GetWeeklyStatisticsOrchestratorHandler : RequestHandlerBase<GetWeeklyStatisticsOrchestrator, Result<WeeklyStatisticsDto>>
{
    public GetWeeklyStatisticsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<WeeklyStatisticsDto>> Handle(GetWeeklyStatisticsOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWeeklyStatisticsQuery(request.DriverId, request.FromDate, request.ToDate), cancellationToken);
        return result;
    }
}


