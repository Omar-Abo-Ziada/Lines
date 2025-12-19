using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TermsAndConditions.DeleteTerms.Orchestrator
{
    public record DeleteTermsOrchestrator( Guid Id) : IRequest<Result<bool>>;

    public class DeleteTermsOrchestratorHandler : RequestHandlerBase<DeleteTermsOrchestrator, Result<bool>>
    {
        public DeleteTermsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<bool>> Handle(DeleteTermsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTermsCommand(request.Id), cancellationToken).ConfigureAwait(false);
            return Result<bool>.Success(result);
        }
    }

}
