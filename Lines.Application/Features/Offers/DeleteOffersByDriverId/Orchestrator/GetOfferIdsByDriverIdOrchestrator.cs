using Lines.Application.Common;
using Lines.Application.Features.Offers.DeleteOffersByDriverId.Queries;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Offers.DeleteOffersByUserId.Orchestrator
{
    public record GetOfferIdsByDriverIdOrchestrator(Guid DriverId) : IRequest<Result<List<Guid>>>;
    
        public class GetOfferIdsByDriverIdOrchestratorHandler(
                       RequestHandlerBaseParameters parameters)
            : RequestHandlerBase<GetOfferIdsByDriverIdOrchestrator, Result<List<Guid>>>(parameters)
    {
        public override async Task<Result<List<Guid>>> Handle(GetOfferIdsByDriverIdOrchestrator request, CancellationToken cancellationToken)
        {
            var offerIds = await _mediator.Send(new GetOfferIdsByDriverIdQuery(request.DriverId), cancellationToken)
                                            .ConfigureAwait(false);

            return Result<List<Guid>>.Success(offerIds);
        }
    }
}
