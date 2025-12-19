using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TermsAndConditions.AddTerms.Orchestrator
{
    public record AddTermsOrchestrator(
        TermsType Type,
        string Header,
        string Content,
        int Order) : IRequest<Result<Guid>>;

    public class AddTermsOrchestratorHandler : RequestHandlerBase<AddTermsOrchestrator, Result<Guid>>
    {
        public AddTermsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<Guid>> Handle(AddTermsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddTermsCommand(request.Type,request.Header,request.Content,request.Order), cancellationToken).ConfigureAwait(false);
            return Result<Guid>.Success(result);
        }
    }

}
