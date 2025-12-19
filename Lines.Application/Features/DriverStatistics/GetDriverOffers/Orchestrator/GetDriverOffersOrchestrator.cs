using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Features.DriverStatistics.GetDriverOffers.Queries;

namespace Lines.Application.Features.DriverStatistics.GetDriverOffers.Orchestrator;

public record GetDriverOffersOrchestrator(Guid DriverId) : IRequest<Result<List<DriverOfferDto>>>;

public class GetDriverOffersOrchestratorHandler : RequestHandlerBase<GetDriverOffersOrchestrator, Result<List<DriverOfferDto>>>
{
    public GetDriverOffersOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<List<DriverOfferDto>>> Handle(GetDriverOffersOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDriverOffersQuery(request.DriverId), cancellationToken);
        return result;
    }
}


