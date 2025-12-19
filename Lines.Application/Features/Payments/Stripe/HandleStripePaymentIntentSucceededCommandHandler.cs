using global::Lines.Domain.Models.Drivers;
using Lines.Application.Features.Notifications.AddNotifications.Commands;
using Lines.Application.Features.Wallets.TopUpWallet.Commands;
using Lines.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;




namespace Lines.Application.Features.Payments.Stripe
{
    // ده بيتنده من الـ Webhook لما Stripe يقول: payment_intent.succeeded
    public record HandleStripePaymentIntentSucceededCommand(string PaymentIntentId, string? StripeChargeId)
        : IRequest<Result<bool>>;

    public class HandleStripePaymentIntentSucceededCommandHandler
        : RequestHandlerBase<HandleStripePaymentIntentSucceededCommand, Result<bool>>
    {
        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Earning> _earningRepository;
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IRepository<WalletTransaction> _walletTransactionRepository;
        private readonly IServiceFeeService _serviceFeeService;
        private readonly ILogger<HandleStripePaymentIntentSucceededCommandHandler> _logger;
        private readonly IFcmNotifier _notifier;
        private readonly IApplicationUserService _appUserService;
        private readonly IRepository<WalletTopUp> _walletTopUpRepository;

        public HandleStripePaymentIntentSucceededCommandHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<Trip> tripRepository,
            IRepository<Payment> paymentRepository,
            IRepository<Earning> earningRepository,
            IRepository<Wallet> walletRepository,
            IRepository<WalletTransaction> walletTransactionRepository,
            IServiceFeeService serviceFeeService,
            ILogger<HandleStripePaymentIntentSucceededCommandHandler> logger,
            IFcmNotifier notifier,
            IApplicationUserService appUserService,
            IRepository<WalletTopUp> walletTopUpRepository)
            : base(parameters)
        {
            _tripRepository = tripRepository;
            _paymentRepository = paymentRepository;
            _earningRepository = earningRepository;
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _serviceFeeService = serviceFeeService;
            _logger = logger;
            _notifier = notifier;
            _appUserService = appUserService;
            _walletTopUpRepository = walletTopUpRepository;
        }

        public override async Task<Result<bool>> Handle(
            HandleStripePaymentIntentSucceededCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.PaymentIntentId))
                {
                    return Result<bool>.Failure(
                        Error.Create("Payment.InvalidPaymentIntent", "PaymentIntentId is required."));
                }

                _logger.LogInformation(
                    "Handling Stripe payment_intent.succeeded for PaymentIntent {PaymentIntentId}",
                    request.PaymentIntentId);

                // 1) نجيب الـ Payment اللي مربوط بالـ PaymentIntent ده
                var paymentQuery = _paymentRepository
                    .Get(p => p.StripePaymentIntentId == request.PaymentIntentId)
                    .Include(p => p.Trip);

                var payment = await paymentQuery.FirstOrDefaultAsync(cancellationToken);

                //if (payment == null)
                //{
                //    _logger.LogWarning(
                //        "Stripe webhook: No Payment found for PaymentIntent {PaymentIntentId}",
                //        request.PaymentIntentId);

                //    // بنرجع Success عشان Stripe مايفضلش يعيد إرسال نفس الـ event
                //    return Result<bool>.Success(true);
                //}

                if (payment == null)
                {
                    // مفيش Trip Payment → احتمال يكون Wallet TopUp
                    var topUp = await _walletTopUpRepository
                        .Get(t => t.PaymentIntentId == request.PaymentIntentId)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (topUp == null)
                    {
                        _logger.LogWarning(
                            "Stripe webhook: No Payment or WalletTopUp found for PaymentIntent {PaymentIntentId}",
                            request.PaymentIntentId);

                        // نرجع Success عشان Stripe ما يفضلش يرنِّي الحدث
                        return Result<bool>.Success(true);
                    }

                    // Idempotency
                    if (topUp.Status == WalletTopUpStatus.Succeeded)
                    {
                        _logger.LogInformation(
                            "Stripe webhook: WalletTopUp {TopUpId} already succeeded for PaymentIntent {PaymentIntentId}",
                            topUp.Id, request.PaymentIntentId);

                        return Result<bool>.Success(true);
                    }

                    // هنا نستخدم TopUpWalletCommand عشان نعمل credit للمحفظة
                    var walletTopUpResult = await _mediator.Send(
                        new TopUpWalletCommand(topUp.UserId, topUp.Amount),
                        cancellationToken);

                    if (!walletTopUpResult.IsSuccess)
                    {
                        //topUp.Status = WalletTopUpStatus.Failed;
                        //topUp.FailureReason = string.Join(";", walletTopUpResult.Errors.Select(e => e.Description));
                        //topUp.FailureReason = walletTopUpResult.Error.Description;
                        topUp.MarkFailed(walletTopUpResult.Error.Description);

                        await _walletTopUpRepository.UpdateAsync(topUp, cancellationToken);
                        await _walletTopUpRepository.SaveChangesAsync(cancellationToken);

                        _logger.LogError(
                            "Stripe webhook: Failed to credit wallet for WalletTopUp {TopUpId}. Errors: {Errors}",
                            topUp.Id, topUp.FailureReason);

                        return Result<bool>.Success(true); // من ناحية Stripe، خلاص الحدث اتعالج
                    }

                    //topUp.Status = WalletTopUpStatus.Succeeded;
                    topUp.MarkSucceeded();

                    //topUp.FailureReason = null;
                    await _walletTopUpRepository.UpdateAsync(topUp, cancellationToken);
                    await _walletTopUpRepository.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Stripe webhook: WalletTopUp {TopUpId} succeeded and wallet credited for user {UserId}",
                        topUp.Id, topUp.UserId);

                    return Result<bool>.Success(true);
                }

                var trip = payment.Trip;
                if (trip == null)
                {
                    // fallback لو الـ Trip مش loaded
                    trip = await _tripRepository
                        .Get(t => t.Id == payment.TripId)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (trip == null)
                    {
                        _logger.LogError(
                            "Stripe webhook: Payment {PaymentId} has no Trip associated (TripId: {TripId})",
                            payment.Id, payment.TripId);

                        return Result<bool>.Success(true);
                    }
                }

                // 2) Idempotency: لو الـ Payment خلاص Completed و Earning موجودة، ما نعملش حاجة
                if (payment.Status == PaymentStatus.Completed)
                {
                    var existingEarning = await _earningRepository
                        .Get(e => e.TripId == trip.Id && e.PaymentId == payment.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (existingEarning != null)
                    {
                        _logger.LogInformation(
                            "Stripe webhook: Payment for Trip {TripId} with PaymentIntent {PaymentIntentId} already processed.",
                            trip.Id, request.PaymentIntentId);

                        return Result<bool>.Success(true);
                    }
                }

                // 3) نعدّل حالة الـ Payment
                payment.Status = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;
                payment.StripeChargeId = request.StripeChargeId;

                var amount = trip.FareAfterRewardApplied ?? trip.Fare;
                payment.UpdateAmount(amount);

                await _paymentRepository.UpdateAsync(payment, cancellationToken);
                await _paymentRepository.SaveChangesAsync(cancellationToken);

                // 4) نعدّل حالة الرحلة
                trip.IsPaid = true;
                trip.CompleteTrip();

                await _tripRepository.UpdateAsync(trip, cancellationToken);
                await _tripRepository.SaveChangesAsync(cancellationToken);

                // 5) نحسب Service Fee + Driver Earning
                decimal serviceFeePercent = await _serviceFeeService
                    .GetApplicableServiceFeePercentAsync(trip.DriverId, cancellationToken);

                //var fare = trip.FareAfterRewardApplied ?? trip.Fare;
                var tips = trip.Tips;
                var fareBeforeDiscount = trip.Fare;

                decimal appFee = fareBeforeDiscount * (serviceFeePercent / 100m);
                decimal driverEarning = fareBeforeDiscount - appFee + tips;

                //decimal appFee = fare * (serviceFeePercent / 100m);
                //decimal driverEarning = fare - appFee + tips;

                if (driverEarning < 0)
                    driverEarning = 0;

                _logger.LogInformation(
                    "Stripe webhook: Trip {TripId} - Fare {Fare}, Tips {Tips}, ServiceFee {ServiceFeePercent}%, AppFee {AppFee}, DriverEarning {DriverEarning}",
                    trip.Id, fareBeforeDiscount, tips, serviceFeePercent, appFee, driverEarning);

                // 6) نتأكد إن فيه Earning واحدة بس للرحلة دي
                var existingEarningForTrip = await _earningRepository
                    .Get(e => e.TripId == trip.Id && e.PaymentId == payment.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingEarningForTrip == null)
                {
                    var earning = new Earning(
                        driverId: trip.DriverId,
                        tripId: trip.Id,
                        amount: driverEarning,
                        paymentId: payment.Id
                    );

                    await _earningRepository.AddAsync(earning, cancellationToken);
                    await _earningRepository.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Stripe webhook: Created earning record {EarningId} for driver {DriverId} with amount {Amount}",
                        earning.Id, trip.DriverId, driverEarning);
                }
                else
                {
                    _logger.LogInformation(
                        "Stripe webhook: Earning already exists for Trip {TripId}, Payment {PaymentId}. Skipping creation.",
                        trip.Id, payment.Id);
                }

                var driverUserId = await _appUserService.GetUserIdByDriverIdAsync(trip.DriverId);
                if (driverUserId == null || driverUserId == Guid.Empty)
                {
                    _logger.LogError("No ApplicationUser found for DriverId {DriverId}", trip.DriverId);
                    return Result<bool>.Success(true); // مش هنفشل الويبهوك، بس سجّل المشكلة
                }

                // 7) تحديث Wallet للسواق + WalletTransaction
                var wallet = await _walletRepository
                    .Get(w => w.UserId == driverUserId.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                if (wallet == null)
                {
                    wallet = new Wallet(driverUserId.Value);
                    await _walletRepository.AddAsync(wallet, cancellationToken);
                    await _walletRepository.SaveChangesAsync(cancellationToken);
                }

                // نحسب reference موحّد عشان نعرف نتجنّب التكرار
                var reference = $"TRIPONLINE-{trip.Id.ToString("N").Substring(0, 12).ToUpper()}";

                var existingWalletTx = await _walletTransactionRepository
                    .Get(t => t.WalletId == wallet.Id && t.Reference == reference)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingWalletTx != null)
                {
                    _logger.LogInformation(
                        "Stripe webhook: WalletTransaction already exists for Trip {TripId}, Wallet {WalletId}. Skipping credit.",
                        trip.Id, wallet.Id);

                    return Result<bool>.Success(true);
                }

                wallet.Credit(driverEarning);
                await _walletRepository.UpdateAsync(wallet, cancellationToken);

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
                    "Stripe webhook: Wallet credited {Amount} for Driver {DriverId}, Wallet {WalletId}, Tx {TxId}",
                    driverEarning, trip.DriverId, wallet.Id, walletTx.Id);


                #region fcm notification

                var passengerUserId = await _appUserService
                     .GetUserIdByPassengerIdAsync(trip.PassengerId);

                if (passengerUserId == null || driverUserId == null)
                {
                    _logger.LogError(
                        "Stripe webhook: Missing UserIds. PassengerUserId: {PassengerUserId}, DriverUserId: {DriverUserId}",
                        passengerUserId, driverUserId);

                    return Result<bool>.Success(true);
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
                    driverUserId.Value, //applicationuserid
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
                    "Stripe webhook: Unexpected error while handling payment_intent.succeeded for PaymentIntent {PaymentIntentId}",
                    request.PaymentIntentId);

                // مهم جدًا نرجع Success في الويبهوك معظم الوقت عشان مايحصلش spam retry من Stripe
                // بس هنا يرجع Failure لـ داخل السيستم عشان تقدر تراقبه
                return Result<bool>.Failure(Error.General);
            }
        }
    }

}
