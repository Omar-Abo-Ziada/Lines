using Lines.Application.Interfaces;
using Lines.Application.Interfaces.Stripe;
using Lines.Application.Shared;
using Lines.Domain.Models.Trips;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Features.Trips.Payments.Commands
{
    // بنرجّح إن الراكب هو اللي بيطلب إنشاء الـ PaymentIntent
    public record CreateTripPaymentIntentCommand(Guid TripId, Guid PassengerId)
        : IRequest<Result<PaymentIntentResult>>;

    public class CreateTripPaymentIntentCommandHandler
        : RequestHandlerBase<CreateTripPaymentIntentCommand, Result<PaymentIntentResult>>
    {
        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<CreateTripPaymentIntentCommandHandler> _logger;

        public CreateTripPaymentIntentCommandHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<Trip> tripRepository,
            IRepository<Payment> paymentRepository,
            IPaymentGateway paymentGateway,
            ILogger<CreateTripPaymentIntentCommandHandler> logger)
            : base(parameters)
        {
            _tripRepository = tripRepository;
            _paymentRepository = paymentRepository;
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public override async Task<Result<PaymentIntentResult>> Handle(
            CreateTripPaymentIntentCommand request,
            CancellationToken cancellationToken)
        {
            // 1) نجيب الرحلة
            var trip = await _tripRepository
                .Get()
                .Include(t => t.Payment)
                .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

            if (trip == null)
                return Result<PaymentIntentResult>.Failure(
                    Error.Create("Trip.NotFound", "Trip not found."));

            // 2) نتأكد إن الراكب صاحب الرحلة فعلاً
            if (trip.PassengerId != request.PassengerId)
                return Result<PaymentIntentResult>.Failure(
                    Error.Create("Trip.Unauthorized",
                        "You are not allowed to pay for this trip."));

            // 3) لو الرحلة ملغية ما نكمّلاش
            if (trip.Status == Domain.Enums.TripStatus.Cancelled)
                return Result<PaymentIntentResult>.Failure(
                    Error.Create("Trip.Cancelled",
                        "Cannot create payment for a cancelled trip."));

            // 4) لو الرحلة مدفوعة خلاص
            if (trip.Payment != null && trip.Payment.Status == Domain.Enums.PaymentStatus.Completed)
                return Result<PaymentIntentResult>.Failure(
                    Error.Create("Payment.AlreadyCompleted",
                        "Payment is already completed for this trip."));

            // 5) نحدد المبلغ (لو فيه خصم من rewards استخدمه)
            var amount = (trip.FareAfterRewardApplied ?? trip.Fare) + trip.Tips;
          
            var currency = trip.Currency;

            if (amount <= 0)
                return Result<PaymentIntentResult>.Failure(
                    Error.Create("Payment.InvalidAmount",
                        "Trip fare must be greater than zero."));

            // 6) ننادي Stripe عبر الـ Gateway
            var stripeResult = await _paymentGateway.CreatePaymentIntentAsync(
                amount,
                currency,
                $"Trip fare for trip #{trip.Id}");

            if (!stripeResult.Success)
            {
                _logger.LogWarning("Failed to create PaymentIntent for Trip {TripId}. Error: {Error}",
                    trip.Id, stripeResult.ErrorMessage);

                return Result<PaymentIntentResult>.Failure(
                    Error.Create("Payment.IntentFailed",
                        stripeResult.ErrorMessage ?? "Failed to create payment intent."));
            }

            // 7) نخزّن معلومات الـ Payment في الـ DB
            Payment payment;

            if (trip.Payment == null)
            {
                payment = new Payment(
                    tripId: trip.Id,
                    paymentMethodId: trip.PaymentMethodId,
                    amount: amount,
                    transactionReference: stripeResult.PaymentIntentId!, // نستخدمه كـ reference
                    status: Domain.Enums.PaymentStatus.NotPaidYet,
                    currency: currency
                );

                payment.StripePaymentIntentId = stripeResult.PaymentIntentId;

                await _paymentRepository.AddAsync(payment, cancellationToken);
                await _paymentRepository.SaveChangesAsync(cancellationToken);

                trip.PaymentId = payment.Id;
                trip.IsPaid = false;

                await _tripRepository.UpdateAsync(trip, cancellationToken);
                await _tripRepository.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // لو فيه Payment قديم نحدّثه بالـ PaymentIntent الجديد
                payment = trip.Payment;

                payment.UpdateAmount(amount);
                payment.UpdateTransactionReference(stripeResult.PaymentIntentId!);
                payment.Status = Domain.Enums.PaymentStatus.NotPaidYet;
                payment.StripePaymentIntentId = stripeResult.PaymentIntentId;

                await _paymentRepository.UpdateAsync(payment, cancellationToken);
                await _paymentRepository.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation(
                "Created PaymentIntent {PaymentIntentId} for Trip {TripId}, Payment {PaymentId}",
                stripeResult.PaymentIntentId, trip.Id, payment.Id);

            // 8) نرجّع نتيجة Stripe للموبايل (client_secret وغيره)
            return Result<PaymentIntentResult>.Success(stripeResult);
        }
    }
}
