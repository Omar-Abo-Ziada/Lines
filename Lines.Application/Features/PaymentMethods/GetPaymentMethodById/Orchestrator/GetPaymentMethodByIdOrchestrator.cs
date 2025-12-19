using Lines.Application.Features.PaymentMethods.GetPaymentMethodById.Queries;
using Lines.Domain.Models.PaymentMethods;

namespace Lines.Application.Features.PaymentMethods.GetPaymentMethodById.Orchestrator;

public record GetPaymentMethodByIdOrchestrator(Guid Id) : IRequest<Result<PaymentMethod>>;
public class GetPaymentMethodByIdOrchestratorHandler : RequestHandlerBase<GetPaymentMethodByIdOrchestrator, Result<PaymentMethod>>
{
    public GetPaymentMethodByIdOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<PaymentMethod>> Handle(GetPaymentMethodByIdOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPaymentMethodByIdQuery(request.Id), cancellationToken).ConfigureAwait(false);
        return Result<PaymentMethod>.Success(result);
    }
}