using Lines.Application.Common;
using Lines.Application.Features.Offers.DeleteOfferById.Orchestrators;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Offers.DeleteOffersByTripRequestId.Orchestrators
{
    public record DeleteOffersByTripRequestIdOrchestrator(Guid TripRequestId) : IRequest<Result<bool>>;
    

    public class DeleteOffersByTripRequestIdOrchestratorHandler(
               RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<DeleteOffersByTripRequestIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteOffersByTripRequestIdOrchestrator request, CancellationToken cancellationToken)
        {
            var offerIds = await _mediator.Send(new GetOfferIdsByTripRequestIdOrchestrator(request.TripRequestId), cancellationToken)
                                            .ConfigureAwait(false);

            foreach (var id in offerIds.Value)
            {
                await _mediator.Send(new DeleteOfferByIdOrchestrator(id))
                               .ConfigureAwait(false);
            }
            return Result<bool>.Success(true);
        }
    }
}
