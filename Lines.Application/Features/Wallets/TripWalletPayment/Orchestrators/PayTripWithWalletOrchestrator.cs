using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TripWalletPayment.Commands;
using Lines.Domain.Shared;
using Lines.Domain.Models.Trips;
using MediatR;

namespace Lines.Application.Features.Wallets.TripWalletPayment.Orchestrators
{
    public record PayTripWithWalletOrchestrator(Guid TripId, Guid PassengerId)
        : IRequest<Result<PayTripWithWalletDto>>;

    public class PayTripWithWalletOrchestratorHandler
     : IRequestHandler<PayTripWithWalletOrchestrator, Result<PayTripWithWalletDto>>
    {
        private readonly IRepository<Trip> _tripRepository;
        private readonly IMediator _mediator;

        public PayTripWithWalletOrchestratorHandler(
            IRepository<Trip> tripRepository,
            IMediator mediator)
        {
            _tripRepository = tripRepository;
            _mediator = mediator;
        }

        public async Task<Result<PayTripWithWalletDto>> Handle(
            PayTripWithWalletOrchestrator request,
            CancellationToken cancellationToken)
        {
            // 1) نجيب الرحلة 
            var trip = await _tripRepository.Get(t => t.Id == request.TripId)
                .FirstOrDefaultAsync(cancellationToken);

            if (trip == null)
                return Result<PayTripWithWalletDto>.Failure(
                    Error.Create("Trip.NotFound", "Trip not found"));

            if (trip.PassengerId != request.PassengerId)
                return Result<PayTripWithWalletDto>.Failure(
                    Error.Create("Trip.Unauthorized", "This trip does not belong to the passenger"));

            var amountToPay = trip.FareAfterRewardApplied ?? trip.Fare;

            // Send the command
            var result = await _mediator.Send(
                new PayTripWithWalletCommand(request.TripId, request.PassengerId),
                cancellationToken);

            if (!result.IsSuccess)
                return Result<PayTripWithWalletDto>.Failure(result.Error);

            // ⚠️ لا تحدث الرحلة هنا — Command نفذها بالفعل
            // لا تعمل Update هنا نهائيًا

            // Return DTO
            return Result<PayTripWithWalletDto>.Success(new PayTripWithWalletDto
            {
                TripId = trip.Id,
                PaidAmount = amountToPay,
                Status = "Paid"
            });
        }
    }


}



