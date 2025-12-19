using Lines.Application.Features.Drivers.GetDriverById.Orchestrators;
using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Constants;

namespace Lines.Application.Features.PaymentMethods.CreateIntent.Orchestrators
{
    public record CreateIntentOrchestrator(Guid UserId, string Role, PaymentMethodType PaymentMethodType) : IRequest<Result<string>>;

    public class CreateIntentOrchestratorHandler(RequestHandlerBaseParameters parameters, IPaymentGateway _paymentGatewayService, IApplicationUserService _applicationUserService)
        : RequestHandlerBase<CreateIntentOrchestrator, Result<string>>(parameters)
    {
        public override async Task<Result<string>> Handle(CreateIntentOrchestrator request,
                                                          CancellationToken cancellationToken)
        {
            var stripeCustomerId = await _applicationUserService.GetStripeCustomerIdByUserIdAsync(request.UserId);
            if (stripeCustomerId is null)
            {
                // get user info to create customer
                var ids = await _applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.UserId);
                if (ids is null)
                    return Result<string>.Failure(Error.NullValue);

                // create stripe customer id
                switch (request.Role)
                {
                    case Roles.Passenger:
                        var passengerResult = await _mediator.Send(new GetPassengerByIdOrchestrator((Guid)ids.PassengerId));
                        if (passengerResult.IsFailure)
                            return Result<string>.Failure(passengerResult.Error);

                        stripeCustomerId = await _paymentGatewayService.CreateCustomerAsync(passengerResult.Value.FirstName
                                           + " " + passengerResult.Value.LastName, passengerResult.Value.Email);
                        break;

                    case Roles.Driver:
                        var driverResult = await _mediator.Send(new GetDriverByIdOrchestrator((Guid)ids.DriverId));
                        if (driverResult.IsFailure)
                            return Result<string>.Failure(driverResult.Error);

                        stripeCustomerId = await _paymentGatewayService.CreateCustomerAsync(driverResult.Value.FirstName
                                           + " " + driverResult.Value.LastName, driverResult.Value.Email);
                        break;
                    case Roles.Admin:
                        ///TODO: implement admin logic after dashboard finished
                        break;

                    default:
                        return Result<string>.Failure(Error.Validation);
                }

                // save stripe customer id in db
                var result = await _applicationUserService.UpdateStripeCustomerIdAsync(request.UserId, stripeCustomerId);

                if (result.IsFailure)
                {
                    return Result<string>.Failure(result.Error);
                }
            }

            // get client secret and return it
            var clientSecret = await _paymentGatewayService.CreatePaymentMethodIntent(stripeCustomerId, request.PaymentMethodType);
            return clientSecret == null ? Result<string>.Failure(Error.NullValue) : Result<string>.Success(clientSecret);
        }
    }
}
