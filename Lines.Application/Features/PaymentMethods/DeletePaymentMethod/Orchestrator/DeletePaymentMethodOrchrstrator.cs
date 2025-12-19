using Lines.Application.Common;
using Lines.Application.Features.PaymentMethods.Command;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.PaymentMethods.DeletePaymentMethod.Orchestrator;

public record DeletePaymentMethodOrchrstrator(Guid Id) : IRequest<Result<bool>>;
public class DeletePaymentMethodOrchrstratorHandler : RequestHandlerBase<DeletePaymentMethodOrchrstrator, Result<bool>>
{
    public DeletePaymentMethodOrchrstratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<bool>> Handle(DeletePaymentMethodOrchrstrator request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePaymentMethodCommand(request.Id), cancellationToken);
        return Result<bool>.Success(true);
    }
}