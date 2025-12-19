using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Orchestrator
{
    public record GetTermsAndConditionsOrchestrator () : IRequest<Result<List<GetTermsAndConditionsDTO>>>;

    public class GetTermsAndConditionsOrchestratorHandler : RequestHandlerBase<GetTermsAndConditionsOrchestrator, Result<List<GetTermsAndConditionsDTO>>>
    {
        public GetTermsAndConditionsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<List<GetTermsAndConditionsDTO>>> Handle(GetTermsAndConditionsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTermsAndConditionsQuery(), cancellationToken).ConfigureAwait(false);
            return Result<List<GetTermsAndConditionsDTO>>.Success(result);
        }
    }
   
}
