using Lines.Application.Features.PaymentMethods.GetPaymentMethodById.Orchestrator;

namespace Lines.Presentation.Endpoints.PaymentMethods.GetPaymentMethodById;

public class GetPaymentMethodByIdEndpoint : BaseController<GetPaymentMethodByIdRequest, GetPaymentMethodByIdResponse>
{
    private readonly BaseControllerParams<GetPaymentMethodByIdRequest> _dependencyCollection;
    public GetPaymentMethodByIdEndpoint(BaseControllerParams<GetPaymentMethodByIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("payment-method/getbyid")]
    public async Task<ApiResponse<GetPaymentMethodByIdResponse>> Generate([FromQuery] GetPaymentMethodByIdRequest request)
    {
        var ValidateResult = await ValidateRequestAsync(request);
        if (!ValidateResult.IsSuccess)
        {
            return ValidateResult;
        }
        var res = await _mediator.Send(new GetPaymentMethodByIdOrchestrator(request.Id)).ConfigureAwait(false);
        return HandleResult<Domain.Models.PaymentMethods.PaymentMethod, GetPaymentMethodByIdResponse>(res);
    }
}