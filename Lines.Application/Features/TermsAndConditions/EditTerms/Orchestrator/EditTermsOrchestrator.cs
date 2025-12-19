using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TermsAndConditions.EditTerms.Orchestrator
{
    public record EditTermsOrchestrator(
        Guid Id,
        string Header,
        string Content,
        int Order) : IRequest<Result<bool>>;

    public class EditTermsOrchestratorHandler : RequestHandlerBase<EditTermsOrchestrator, Result<bool>>
    {
        public EditTermsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<bool>> Handle(EditTermsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new EditTermsCommand(request.Id,request.Header,request.Content,request.Order), cancellationToken).ConfigureAwait(false);
            return Result<bool>.Success(result);
        }
    }

}
