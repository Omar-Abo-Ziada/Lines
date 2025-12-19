using Lines.Application.Common;
using Lines.Application.Features.Offers.DeleteOfferById.Orchestrators;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Offers.DeleteOffersByUserId.Orchestrator
{
    public record DeleteOffersByDriverIdOrchestrator(Guid DriverId) : IRequest<Result<bool>>;

    public class DeleteOffersByDriverIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteOffersByDriverIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteOffersByDriverIdOrchestrator request, CancellationToken cancellationToken)
        {

          var offerIds = await _mediator.Send(new GetOfferIdsByDriverIdOrchestrator(request.DriverId), cancellationToken)
                                        .ConfigureAwait(false);


            foreach(var id in offerIds.Value)
            {
                await _mediator.Send(new DeleteOfferByIdOrchestrator(id))
                               .ConfigureAwait(false);
            }
            return Result<bool>.Success(true);
        }
    }
} 