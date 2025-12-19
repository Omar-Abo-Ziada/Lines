using Lines.Application.Common;
using Lines.Application.Features.Offers.DeleteOfferById.Commands;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lines.Application.Features.Offers.DeleteOfferById.Orchestrators
{
    public record DeleteOfferByIdOrchestrator(Guid OfferId) : IRequest<Result<bool>>;

    public class DeleteOfferByIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteOfferByIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteOfferByIdOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOfferByIdCommand(request.OfferId), cancellationToken)
                           .ConfigureAwait(false);

            return Result<bool>.Success(true);
        }
    }
}
