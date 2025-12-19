
using global::Lines.Application.Features.Payments.Stripe;
using Lines.Application.Features.Payments.DriverStripeAccounts;
using Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
  



namespace Lines.Presentation.Endpoints.Payments.Stripe
{
    [AllowAnonymous] // مهم: Stripe مش هيبعت JWT
    [ApiController]
    [Route("api/webhooks/stripe")]
    public class StripeWebhookEndpoint : ControllerBase
    {
        private readonly ILogger<StripeWebhookEndpoint> _logger;
        private readonly IMediator _mediator;
        private readonly StripeOptions _stripeOptions;

        public StripeWebhookEndpoint(
            ILogger<StripeWebhookEndpoint> logger,
            IMediator mediator,
            IOptions<StripeOptions> stripeOptions)
        {
            _logger = logger;
            _mediator = mediator;
            _stripeOptions = stripeOptions.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var sigHeader = Request.Headers["Stripe-Signature"];
            if (string.IsNullOrEmpty(sigHeader))
            {
                _logger.LogWarning("Stripe webhook: Missing Stripe-Signature header.");
                return BadRequest();
            }

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    sigHeader,
                    _stripeOptions.WebhookSecret
                );
            }
            catch (StripeException ex)
            {
                _logger.LogWarning(ex, "Stripe webhook: Signature verification failed.");
                return BadRequest();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Stripe webhook: Failed to parse event.");
                return BadRequest();
            }

            _logger.LogInformation(
                "Stripe webhook received. Type: {Type}, Id: {Id}",
                stripeEvent.Type, stripeEvent.Id);

            try
            {
                switch (stripeEvent.Type)
                {
                    // ==============================
                    // 1) PaymentIntent Succeeded
                    // ==============================
                    //case "payment_intent.succeeded":
                    //    //case Stripe.Events.PaymentIntentSucceeded:
                    //    {
                    //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    //        if (paymentIntent == null)
                    //        {
                    //            _logger.LogError("Stripe webhook: payment_intent.succeeded event with null PaymentIntent object.");
                    //            break;
                    //        }
                    //        var chargeId = paymentIntent.Charges?.Data?.FirstOrDefault()?.Id;


                    //        _logger.LogInformation(
                    //            "Stripe webhook: payment_intent.succeeded. Id: {PaymentIntentId}, Amount: {Amount}, Currency: {Currency}",
                    //            paymentIntent.Id,
                    //            paymentIntent.Amount,
                    //            paymentIntent.Currency);

                    //        var result = await _mediator.Send(
                    //            new HandleStripePaymentIntentSucceededCommand(paymentIntent.Id));

                    //        if (result.IsFailure)
                    //        {
                    //            _logger.LogError(
                    //                "Stripe webhook: Failed to handle payment_intent.succeeded for PaymentIntent {PaymentIntentId}. Error: {ErrorCode} - {ErrorDescription}",
                    //                paymentIntent.Id,
                    //                result.Error.Code,
                    //                result.Error.Description);
                    //        }

                    //        break;
                    //    }

                    case "payment_intent.succeeded":
                        {
                            var pi = stripeEvent.Data.Object as PaymentIntent;
                            if (pi == null)
                            {
                                _logger.LogError("Stripe webhook: payment_intent.succeeded with null PaymentIntent");
                                break;
                            }

                            // ⚡ إعادة جلب PaymentIntent مع Expand للـ latest_charge
                            var piService = new PaymentIntentService();
                            pi = await piService.GetAsync(pi.Id, new PaymentIntentGetOptions
                            {
                                Expand = new List<string> { "latest_charge" }
                            });

                            var chargeId = pi.LatestChargeId;

                            _logger.LogInformation(
                                "Stripe webhook: payment_intent.succeeded. PI: {Id}, ChargeId: {ChargeId}, Amount: {Amount}",
                                pi.Id, chargeId, pi.Amount);

                            await _mediator.Send(
                                new HandleStripePaymentIntentSucceededCommand(
                                    pi.Id,
                                    chargeId
                                ));

                            break;
                        }


                    // ==============================
                    // 2) PaymentIntent Failed
                    // ==============================
                    //case Events.PaymentIntentPaymentFailed:
                    case "payment_intent.payment_failed":

                        {
                            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                            var errorMsg = paymentIntent?.LastPaymentError?.Message;

                            _logger.LogWarning(
                                "Stripe webhook: payment_intent.payment_failed. Id: {PaymentIntentId}, Error: {Error}",
                                paymentIntent?.Id,
                                errorMsg);

                            // هنا ممكن قدّام تعمل Command مخصوص تـ mark Payment كـ Failed
                            // أو تحط Log في جدول Notifications للـ passenger

                            break;
                        }

                    // ==============================
                    // 3) Charge Refunded
                    // ==============================
                    //case Events.ChargeRefunded:
                    case "charge.refunded":
                        {
                            var charge = stripeEvent.Data.Object as Charge;
                            _logger.LogInformation(
                                "Stripe webhook: charge.refunded. ChargeId: {ChargeId}, AmountRefunded: {AmountRefunded}",
                                charge?.Id,
                                charge?.AmountRefunded);

                            // قدّام:
                            // - تلاقي Payment عن طريق charge.Id (لو خزّناه)
                            // - تعمل RefundResult + WalletAdjustment

                            break;
                        }


                    // =========================
                    // STRIPE CONNECT 
                    // =========================
                    case "account.updated":
                        {
                            var account = stripeEvent.Data.Object as Account;
                            if (account == null)
                            {
                                _logger.LogError("Stripe webhook: account.updated with null Account");
                                break;
                            }

                            await _mediator.Send(
                                new HandleStripeAccountUpdatedOrchestrator(
                                    account.Id,
                                    account.ChargesEnabled,
                                    account.PayoutsEnabled,
                                    account.DetailsSubmitted
                                )
                            );

                            break;
                        }


                    default:
                        _logger.LogInformation(
                            "Stripe webhook: Unhandled event type: {Type}",
                            stripeEvent.Type);
                        break;
                }

                // Stripe محتاج 2xx عشان يعتبر إن الويبهوك اتعالج بنجاح
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex,
                    "Stripe webhook: Unexpected error while handling event {EventId} of type {Type}",
                    stripeEvent.Id,
                    stripeEvent.Type);

                // ممكن ترجّع 500 عشان Stripe يعيد المحاولة
                // لكن خلي بالك من idempotency في الـ Command
                return StatusCode(500);
            }
        }
    }
}


