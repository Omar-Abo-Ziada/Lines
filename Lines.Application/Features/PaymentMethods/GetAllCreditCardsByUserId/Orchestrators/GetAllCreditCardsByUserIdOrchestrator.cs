using Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.DTOs;
using Lines.Application.Interfaces.Stripe;

namespace Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.Orchestrators
{
    public record GetAllCreditCardsByUserIdOrchestrator(Guid userId) : IRequest<Result<List<PaymentGatewayCreditCardDto>>>;


    public class GetAllCreditCardsByUserIdOrchestratorHandler : RequestHandlerBase<GetAllCreditCardsByUserIdOrchestrator, Result<List<PaymentGatewayCreditCardDto>>>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly IApplicationUserService _applicationUserService;
        public GetAllCreditCardsByUserIdOrchestratorHandler(RequestHandlerBaseParameters parameters, IPaymentGateway paymentGateway,
            IApplicationUserService applicationUserService) : base(parameters)
        {
            _paymentGateway = paymentGateway;
            _applicationUserService = applicationUserService;
        }

        public override async Task<Result<List<PaymentGatewayCreditCardDto>>> Handle(GetAllCreditCardsByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            var customerId = await _applicationUserService.GetStripeCustomerIdByUserIdAsync(request.userId);
            if (customerId is null)
            {
                return Result<List<PaymentGatewayCreditCardDto>>.Failure(PaymentMethodErrors.CustomerIdNotFoundError("This user does not have customer id"));
            }

            var paymentMethods = _paymentGateway.GetCreditCardsByCustomerId(customerId);
            
            // assign default
            string defaultPaymentMethodId = _paymentGateway.GetCustomerDefaultPaymentMethodIdByCustomerId(customerId);
            foreach (var paymentMethod in paymentMethods)
            {
                paymentMethod.IsDefault = paymentMethod.Id == defaultPaymentMethodId;
            }

            return Result<List<PaymentGatewayCreditCardDto>>.Success(paymentMethods);  
        }
    }
}
