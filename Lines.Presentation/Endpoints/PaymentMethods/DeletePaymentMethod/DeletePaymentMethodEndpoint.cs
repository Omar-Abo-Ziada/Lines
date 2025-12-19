using Lines.Application.Features.PaymentMethods.DeletePaymentMethod.Orchestrator;

namespace Lines.Presentation.Endpoints.PaymentMethods.DeletePaymentMethod;

public class DeletePaymentMethodEndpoint : BaseController<DeletePaymentMethodRequest, ApiResponse<bool>>
{
    private readonly BaseControllerParams<DeletePaymentMethodRequest> _dependencyCollection;
    public DeletePaymentMethodEndpoint(BaseControllerParams<DeletePaymentMethodRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpDelete("payment-method/delete")]
    public async Task<ApiResponse<bool>> Generate([FromBody] DeletePaymentMethodRequest request)
    {
        var res = await _mediator.Send(new DeletePaymentMethodOrchrstrator(request.Id)).ConfigureAwait(false);
        return HandleResult<bool, bool>(res);
    }
}