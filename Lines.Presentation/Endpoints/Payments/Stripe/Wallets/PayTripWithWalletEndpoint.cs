using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TripWalletPayment.Orchestrators;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Payments.Stripe.Wallets
{
    [Authorize]
    public class PayTripWithWalletEndpoint
        : BaseController<PayTripWithWalletRequest, PayTripWithWalletResponse>
    {
        private readonly BaseControllerParams<PayTripWithWalletRequest> _dependencyCollection;

        public PayTripWithWalletEndpoint(
            BaseControllerParams<PayTripWithWalletRequest> dependencyCollection
        ) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        // POST /trips/{tripId}/pay-with-wallet
        [HttpPost("trips/{tripId:guid}/pay-with-wallet")]
        public async Task<ApiResponse<PayTripWithWalletResponse>> Pay(Guid tripId)
        {
            // 1) نجيب passengerId من الـ JWT
            var passengerId = GetCurrentPassengerId();
            if (passengerId == Guid.Empty)
            {
                return HandleResult<PayTripWithWalletDto, PayTripWithWalletResponse>(
                    Result<PayTripWithWalletDto>.Failure(
                        new Lines.Application.Shared.Error(
                            "UNAUTHORIZED",
                            "User not authenticated",
                            Lines.Application.Shared.ErrorType.Validation)));
            }

            // 2) ننده الـ Orchestrator
            var result = await _mediator.Send(
                new PayTripWithWalletOrchestrator(tripId, passengerId));

            // 3) نرجّع ApiResponse موحّد زي باقي السيستم
            return HandleResult<PayTripWithWalletDto, PayTripWithWalletResponse>(result);
        }
    }
}
