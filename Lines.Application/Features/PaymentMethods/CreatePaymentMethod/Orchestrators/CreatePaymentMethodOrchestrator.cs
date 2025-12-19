using Lines.Application.Features.PaymentMethods.CreatePaymentMethod.Commands;
using Lines.Application.Features.PaymentMethods.GetAllPaymentMethodsByUserId.Queries;
using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Models.PaymentMethods;

namespace Lines.Application.Features.PaymentMethods.CreatePaymentMethod.Orchestrators
{
    public record CreatePaymentMethodOrchestrator(Guid userId, string paymentMethodId, PaymentMethodType paymentMethodType, bool isDefault = false) : IRequest<bool>;


    public class AttachPaymentMethodToUserOrchestratorHandler :
        RequestHandlerBase<CreatePaymentMethodOrchestrator, bool>
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPaymentGateway _paymentGatewayService;

        public AttachPaymentMethodToUserOrchestratorHandler(IRepository<PaymentMethod> paymentMethodRepository, IApplicationUserService applicationUserService,
            IPaymentGateway paymentGatewayService, RequestHandlerBaseParameters parameters) : base(parameters)
        {
            _applicationUserService = applicationUserService;
            _paymentGatewayService = paymentGatewayService;
        }

        public override async Task<bool> Handle(CreatePaymentMethodOrchestrator request, CancellationToken cancellationToken)
        {
            // 1- get customer id 
            var customerId = await _applicationUserService.GetStripeCustomerIdByUserIdAsync(request.userId);
            if (customerId is null)
            {
                return false;   // do not create, front must call the get client secret first
            }

            // 2- create payment method record in db and attach to user 
            var result = await _mediator.Send(new CreatePaymentMethodCommand(request.paymentMethodId, customerId, request.paymentMethodType, request.isDefault, request.userId));
            
            // 3- detach from stripe in failure
            if(result.IsFailure)
            {
                await _paymentGatewayService.DetachPaymentMethodFromCustomerAsync(request.paymentMethodId); // rollback attach
                return false;
            }

            // 4- set as default in stripe
            if (request.isDefault)
            {
                // set new default in stripe
                await _paymentGatewayService.SetDefaultPaymentMethodAsync(customerId, request.paymentMethodId);
            }

            return true;
        }
    }




}
