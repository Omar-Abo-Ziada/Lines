using global::Lines.Application.Interfaces.Stripe;
using global::Lines.Domain.Models.Drivers;
using Lines.Application.Features.Notifications.AddNotifications.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace Lines.Application.Features.Payments.Stripe
{


    // Passenger بيأكد إن الدفع نجح فعلاً من Stripe
    //يعتبر بديل لل webhook
    public record ConfirmStripeTripPaymentCommand(
        Guid TripId,
        Guid PassengerId,
        string PaymentIntentId
    ) : IRequest<Result<bool>>;

    public class ConfirmStripeTripPaymentCommandHandler
        : RequestHandlerBase<ConfirmStripeTripPaymentCommand, Result<bool>>
    {
        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Earning> _earningRepository;
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IRepository<WalletTransaction> _walletTransactionRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IServiceFeeService _serviceFeeService;
        private readonly ILogger<ConfirmStripeTripPaymentCommandHandler> _logger;
        private readonly IFcmNotifier _notifier;
        private readonly IApplicationUserService _appUserService;

        public ConfirmStripeTripPaymentCommandHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<Trip> tripRepository,
            IRepository<Payment> paymentRepository,
            IRepository<Earning> earningRepository,
            IRepository<Wallet> walletRepository,
            IRepository<WalletTransaction> walletTransactionRepository,
            IPaymentGateway paymentGateway,
            IServiceFeeService serviceFeeService,
            ILogger<ConfirmStripeTripPaymentCommandHandler> logger,
            IFcmNotifier notifier,
            IApplicationUserService appUserService)
            : base(parameters)
        {
            _tripRepository = tripRepository;
            _paymentRepository = paymentRepository;
            _earningRepository = earningRepository;
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _paymentGateway = paymentGateway;
            _serviceFeeService = serviceFeeService;
            _logger = logger;
            _notifier = notifier;
            _appUserService = appUserService;
        }

        public override async Task<Result<bool>> Handle(
            ConfirmStripeTripPaymentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 1) نجيب الرحلة
                var trip = await _tripRepository
                    .Get()
                    .Include(t => t.Payment)
                    .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

                if (trip == null)
                {
                    return Result<bool>.Failure(
                        Error.Create("Trip.NotFound", "Trip not found."));
                }

                // 2) نتأكد إن الراكب صاحب الرحلة فعلاً
                if (trip.PassengerId != request.PassengerId)
                {
                    return Result<bool>.Failure(
                        Error.Create("Trip.Unauthorized", "You are not allowed to confirm payment for this trip."));
                }

                // 3) نتأكد إن الـ PaymentIntentId مبعوت
                if (string.IsNullOrWhiteSpace(request.PaymentIntentId))
                {
                    return Result<bool>.Failure(
                        Error.Create("Payment.InvalidPaymentIntent", "PaymentIntentId is required."));
                }

                // 4) نسأل Stripe عن حالة الـ PaymentIntent
                var stripeResult = await _paymentGateway.ConfirmPaymentAsync(request.PaymentIntentId);

                if (!stripeResult.Success && stripeResult.RequiresAction)
                {
                    // محتاج 3D Secure أو Action من العميل
                    return Result<bool>.Failure(
                        Error.Create("Payment.RequiresAction",
                            "Payment requires additional authentication (3D Secure)."));
                }

                if (!stripeResult.Success)
                {
                    // Failed / Canceled / ...
                    return Result<bool>.Failure(
                        Error.Create("Payment.NotSucceeded",
                            stripeResult.ErrorMessage ?? "Payment did not succeed."));
                }

                if (!string.Equals(stripeResult.Status, "succeeded", StringComparison.OrdinalIgnoreCase))
                {
                    return Result<bool>.Failure(
                        Error.Create("Payment.InvalidStatus",
                            $"PaymentIntent status is '{stripeResult.Status}', not 'succeeded'."));
                }

                // 5) نلاقي الـ Payment في الداتا بيز عن طريق StripePaymentIntentId
                var payment = await _paymentRepository
                    .Get(p => p.TripId == trip.Id && p.StripePaymentIntentId == request.PaymentIntentId)
                    .FirstOrDefaultAsync(cancellationToken);
                var amount = trip.FareAfterRewardApplied ?? trip.Fare;

                if (payment == null)
                {
                    // لو مش موجودة لأي سبب (مثلاً حصلت مشكلة قبل الحفظ) نخلق Payment جديدة
                    payment = new Payment(
                        tripId: trip.Id,
                        paymentMethodId: trip.PaymentMethodId,
                        //amount: trip.Fare,
                        amount: amount,
                        transactionReference: request.PaymentIntentId,
                        status: PaymentStatus.Completed,
                        currency: trip.Currency
                    );

                    payment.StripePaymentIntentId = request.PaymentIntentId;
                    payment.PaidAt = DateTime.UtcNow;

                    await _paymentRepository.AddAsync(payment, cancellationToken);
                    await _paymentRepository.SaveChangesAsync(cancellationToken);

                    trip.PaymentId = payment.Id;
                }
                else
                {
                    // لو موجودة: نتأكد إننا مش بنكرر الـ Complete
                    if (payment.Status == PaymentStatus.Completed)
                    {
                        _logger.LogInformation(
                            "Payment for Trip {TripId} with PaymentIntent {PaymentIntentId} already completed.",
                            trip.Id, request.PaymentIntentId);

                        // Idempotent success
                        return Result<bool>.Success(true);
                    }

                    payment.Status = PaymentStatus.Completed;
                    payment.PaidAt = DateTime.UtcNow;
                    payment.UpdateAmount(amount);

                    await _paymentRepository.UpdateAsync(payment, cancellationToken);
                    await _paymentRepository.SaveChangesAsync(cancellationToken);
                }

                // نعدل حالة الـ Trip
                trip.IsPaid = true;
                trip.CompleteTrip();
                await _tripRepository.UpdateAsync(trip, cancellationToken);
                await _tripRepository.SaveChangesAsync(cancellationToken);

                // ============================================================
                // 6) حساب Service Fee + Driver Earning وإضافة Earning + Wallet
                // ============================================================
                decimal serviceFeePercent = await _serviceFeeService
                    .GetApplicableServiceFeePercentAsync(trip.DriverId, cancellationToken);

                // fare الحقيقي بعد rewards لو عندك
                //var fare = trip.FareAfterRewardApplied ?? trip.Fare;
                //var tips = trip.Tips;

                //decimal appFee = fare * (serviceFeePercent / 100m);
                //decimal driverEarning = fare - appFee + tips;

                var tips = trip.Tips;
                var fareBeforeDiscount = trip.Fare;

                decimal appFee = fareBeforeDiscount * (serviceFeePercent / 100m);
                decimal driverEarning = fareBeforeDiscount - appFee + tips;

                if (driverEarning < 0)
                {
                    driverEarning = 0; // بس احتياط
                }

                _logger.LogInformation(
                    "ConfirmStripeTripPayment: Trip {TripId} - Fare {Fare}, Tips {Tips}, ServiceFee {ServiceFeePercent}%, AppFee {AppFee}, DriverEarning {DriverEarning}",
                    trip.Id, fareBeforeDiscount, tips, serviceFeePercent, appFee, driverEarning);

                 var earning = new Earning(
                    driverId: trip.DriverId,
                    tripId: trip.Id,
                    amount: driverEarning,
                    paymentId: payment.Id
                );

                await _earningRepository.AddAsync(earning, cancellationToken);
                await _earningRepository.SaveChangesAsync(cancellationToken);

                var driverUserId = await _appUserService.GetUserIdByDriverIdAsync(trip.DriverId);
                if (driverUserId == null || driverUserId == Guid.Empty)
                {
                    _logger.LogError("No ApplicationUser found for DriverId {DriverId}", trip.DriverId);
                    return Result<bool>.Success(true); // مش هنفشل الويبهوك، بس سجّل المشكلة
                }

                // 6.2 تحديث Wallet للسواق
                var wallet = await _walletRepository
                    .Get(w => w.UserId == driverUserId.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                if (wallet == null)
                {
                    wallet = new Wallet(driverUserId.Value);
                    await _walletRepository.AddAsync(wallet, cancellationToken);
                    await _walletRepository.SaveChangesAsync(cancellationToken);
                }

                wallet.Credit(driverEarning);
                await _walletRepository.UpdateAsync(wallet, cancellationToken);

                var reference = $"TRIPONLINE-{trip.Id.ToString("N").Substring(0, 12).ToUpper()}";

                var walletTx = new WalletTransaction(
                    walletId: wallet.Id,
                    amount: driverEarning,
                    type: TransactionType.Credit,
                    transactionCategory: WalletTransactionCategory.TripOnlineNet, 
                    reference: reference,
                    description: $"Online trip earning for trip {trip.Id}"
                );

                await _walletTransactionRepository.AddAsync(walletTx, cancellationToken);
                await _walletTransactionRepository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Completed Stripe payment confirmation for Trip {TripId}, Driver {DriverId}, Earning {EarningId}, Wallet {WalletId}",
                    trip.Id, trip.DriverId, earning.Id, wallet.Id);


                #region fcm notification

                var passengerUserId = await _appUserService
                    .GetUserIdByPassengerIdAsync(trip.PassengerId);
 
                if (passengerUserId == null || driverUserId == null)
                {
                    _logger.LogError(
                        "Cannot send notifications. PassengerUserId: {PassengerUserId}, DriverUserId: {DriverUserId}",
                        passengerUserId, driverUserId);

                    return Result<bool>.Success(true); // ما نكسّرش الدفع
                }


                //--------------------------------------
                //  Passenger Notification
                //--------------------------------------
                await _notifier.SendToUserAsync(
                 passengerUserId.Value,
                    "تم الدفع",
                    $"تم دفع رحلتك رقم {trip.Id} بنجاح.",
                    new { tripId = trip.Id }
                );

                await _mediator.Send(new AddNotificationsCommand(
                 passengerUserId.Value,
                    "تم الدفع",
                    $"تم دفع رحلتك رقم {trip.Id} بنجاح.",
                    NotificationType.PaymentSuccess
                ));

                //--------------------------------------
                //  Driver Notification
                //--------------------------------------  
                await _notifier.SendToUserAsync(
                    driverUserId.Value,
                    "دخل جديد",
                    $"تم إضافة {driverEarning} جنيه إلى محفظتك.",
                    new { tripId = trip.Id, earning = driverEarning }
                );

                await _mediator.Send(new AddNotificationsCommand(
                    driverUserId.Value,
                    "دخل جديد",
                    $"تم إضافة {driverEarning} جنيه إلى محفظتك.",
                    NotificationType.DriverEarning
                ));
                #endregion


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while confirming Stripe payment for Trip {TripId}", request.TripId);

                return Result<bool>.Failure(Error.General);
            }
        }
    }

}
