using Lines.Application.Common;
using Lines.Application.Features.Trips;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.CodeDom;

namespace Lines.Application.Features.Trips.CollectTripFare.Commands;

public record CollectTripFareCommand(Guid TripId, Guid DriverId) : IRequest<Result<bool>>;

public class CollectTripFareCommandHandler(
    RequestHandlerBaseParameters parameters, 
    IRepository<Trip> repository, 
    IRepository<Payment> paymentRepository,
    IRepository<Earning> earningRepository,
    IServiceFeeService serviceFeeService,
    ILogger<CollectTripFareCommandHandler> logger)
    : RequestHandlerBase<CollectTripFareCommand, Result<bool>>(parameters)
{
    private readonly ILogger<CollectTripFareCommandHandler> _logger = logger;

    public override async Task<Result<bool>> Handle(CollectTripFareCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Load trip with payment
            var trip = await repository.Get()
                .Include(t => t.Payment)
                .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

            if (trip == null)
            {
                return Result<bool>.Failure(TripErrors.TripNotFound);
            }

            // Validate driver owns the trip
            if (trip.DriverId != request.DriverId)
            {
                return Result<bool>.Failure(TripErrors.UnauthorizedTripAccess);
            }

            // Check if payment already collected
            if (trip.Payment != null && trip.Payment.Status == PaymentStatus.Completed)
            {
                return Result<bool>.Failure(TripErrors.PaymentAlreadyCollected);
            }

            // Create or update payment
            Payment payment;
            if (trip.Payment == null)
            {
                // Create new payment
                payment = new Payment(
                    trip.Id,
                    trip.PaymentMethodId,
                    trip.Fare,
                    $"TXN-{trip.Id}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    PaymentStatus.Completed,
                    trip.Currency
                );

                await paymentRepository.AddAsync(payment, cancellationToken);
                await paymentRepository.SaveChangesAsync(cancellationToken);
                trip.PaymentId = payment.Id;
                // trip.SetPayment(payment);

            }
            else
            {
                // Update existing payment
                payment = trip.Payment;
                payment.Status = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;
            }

            // ============================================================
            // APPLY OFFER-BASED SERVICE FEE AND CALCULATE DRIVER EARNING
            // ============================================================
            
            // Get applicable service fee (considers active offers)
            decimal serviceFeePercent = await serviceFeeService.GetApplicableServiceFeePercentAsync(
                trip.DriverId, 
                cancellationToken);

            // Calculate service fee amount and driver earning
            decimal appFee = trip.Fare * (serviceFeePercent / 100);
            decimal driverEarning = trip.Fare - appFee + trip.Tips;

            _logger.LogInformation(
                "Applied service fee {ServiceFeePercent}% to trip {TripId}. " +
                "Fare: {Fare}, Tips: {Tips}, App Fee: {AppFee}, Driver Earning: {DriverEarning}",
                serviceFeePercent, trip.Id, trip.Fare, trip.Tips, appFee, driverEarning);

            // Create earning record for the driver
            var earning = new Earning(
                driverId: trip.DriverId,
                tripId: trip.Id,
                amount: driverEarning,
                paymentId: payment.Id
            );

            await earningRepository.AddAsync(earning, cancellationToken);
            await earningRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Created earning record {EarningId} for driver {DriverId} with amount {Amount}",
                earning.Id, trip.DriverId, driverEarning);

            await repository.UpdateAsync(trip, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(TripErrors.PaymentAlreadyCollected);
        }
       
    }
}
