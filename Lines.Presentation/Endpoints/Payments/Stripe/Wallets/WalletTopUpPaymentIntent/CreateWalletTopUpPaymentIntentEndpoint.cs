
using global::Lines.Application.Features.Wallets.DTOs;
using global::Lines.Application.Features.Wallets.TopUpOnline.Orchestrators;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TopUpOnline.Orchestrators;
using Lines.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Lines.Presentation.Endpoints.Payments.Stripe.Wallets.WalletTopUpPaymentIntent
{

    [Authorize]
    [Route("api/wallets")]
    public class CreateWalletTopUpPaymentIntentEndpoint
        : BaseController<CreateWalletTopUpPaymentIntentRequest, CreateWalletTopUpPaymentIntentResponse>
    {
        public CreateWalletTopUpPaymentIntentEndpoint(
            BaseControllerParams<CreateWalletTopUpPaymentIntentRequest> parameters)
            : base(parameters)
        {
        }
        // شحن المحفظة - إنشاء نية دفع لشحن المحفظة
        [HttpPost("create-topup-intent")]
        public async Task<ApiResponse<CreateWalletTopUpPaymentIntentResponse>> CreateIntent(
             [FromBody] CreateWalletTopUpPaymentIntentRequest request)
        {
            var userId = GetCurrentUserId();

            var result = await _mediator.Send(
                new CreateWalletTopUpPaymentIntentOrchestrator(
                    userId,
                    request.Amount,
                    request.Currency
                ));

            return HandleResult<CreateWalletTopUpPaymentIntentDto, CreateWalletTopUpPaymentIntentResponse>(result);
        }

       
    }
}


