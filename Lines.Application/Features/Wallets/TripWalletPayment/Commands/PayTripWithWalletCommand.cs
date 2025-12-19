using Lines.Application.Common;
using Lines.Application.Features.Notifications.AddNotifications.Commands;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Lines.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Features.Wallets.TripWalletPayment.Commands;

public record PayTripWithWalletCommand(Guid TripId, Guid PassengerId)
    : IRequest<Result<bool>>;

// الراكب يدفع فلوس الرحلة من المحفظة الخاصة بيه بديل عن الدفع بالفيزا
public class PayTripWithWalletCommandHandler
    : RequestHandlerBase<PayTripWithWalletCommand, Result<bool>>
{
    private readonly IRepository<Trip> _tripRepository;
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<Earning> _earningRepository;
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IRepository<WalletTransaction> _walletTransactionRepository;
    private readonly IServiceFeeService _serviceFeeService;
    private readonly IFcmNotifier _notifier;
    private readonly IApplicationUserService _appUserService;
    private readonly ILogger<PayTripWithWalletCommandHandler> _logger;

    public PayTripWithWalletCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Trip> tripRepository,
        IRepository<Payment> paymentRepository,
        IRepository<Earning> earningRepository,
        IRepository<Wallet> walletRepository,
        IRepository<WalletTransaction> walletTransactionRepository,
        IServiceFeeService serviceFeeService,
        IFcmNotifier notifier,
        IApplicationUserService appUserService,
        ILogger<PayTripWithWalletCommandHandler> logger)
        : base(parameters)
    {
        _tripRepository = tripRepository;
        _paymentRepository = paymentRepository;
        _earningRepository = earningRepository;
        _walletRepository = walletRepository;
        _walletTransactionRepository = walletTransactionRepository;
        _serviceFeeService = serviceFeeService;
        _notifier = notifier;
        _appUserService = appUserService;
        _logger = logger;
    }

    public override async Task<Result<bool>> Handle(
        PayTripWithWalletCommand request,
        CancellationToken cancellationToken)
    {
        // 1) نجيب الرحلة
        var trip = await _tripRepository
            .Get()
            .Include(t => t.Payment)
            .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

        if (trip == null)
            return Result<bool>.Failure(Error.Create("Trip.NotFound", "Trip not found."));

        // 2) تأكيد إن الراكب صاحب الرحلة
        if (trip.PassengerId != request.PassengerId)
            return Result<bool>.Failure(
                Error.Create("Trip.Unauthorized", "You are not allowed to pay for this trip."));

        // 3) Idempotency: لو الرحلة مدفوعة بالفعل
        if (trip.IsPaid || (trip.Payment != null && trip.Payment.Status == PaymentStatus.Completed))
        {
            _logger.LogInformation("Trip {TripId} already paid.", trip.Id);
            return Result<bool>.Success(true);
        }

        // 4) نجيب AppUser الخاص بالراكب
        var passengerUserId = await _appUserService.GetUserIdByPassengerIdAsync(trip.PassengerId);
        if (passengerUserId == null || passengerUserId == Guid.Empty)
            return Result<bool>.Failure(
                Error.Create("User.NotFound", "No user found for this passenger."));

        // 5) نجيب المحفظة
        var passengerWallet = await _walletRepository
            .Get(w => w.UserId == passengerUserId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (passengerWallet == null)
        {
            passengerWallet = new Wallet(passengerUserId.Value);
            await _walletRepository.AddAsync(passengerWallet, cancellationToken);
            await _walletRepository.SaveChangesAsync(cancellationToken);
        }

        var fareBeforeDiscount = trip.Fare;
        var amountToCharge = trip.FareAfterRewardApplied ?? fareBeforeDiscount;

        if (!passengerWallet.HasSufficientBalance(amountToCharge))
        {
            return Result<bool>.Failure(
                Error.Create("Wallet.InsufficientBalance",
                    $"Insufficient wallet balance. Required: {amountToCharge}, Available: {passengerWallet.Balance}"));
        }

        // 6) نعمل Debit للمحفظة + WalletTransaction للراكب
        passengerWallet.Debit(amountToCharge);
        await _walletRepository.UpdateAsync(passengerWallet, cancellationToken);

        var passengerReference = $"TRIPWALLET-{trip.Id.ToString("N").Substring(0, 12).ToUpper()}";

        var passengerTx = new WalletTransaction(
            walletId: passengerWallet.Id,
            amount: amountToCharge,
            type: TransactionType.Debit,
            transactionCategory: WalletTransactionCategory.TripEarning, // ممكن تعمل enum خاص لخصم قيمة الرحلة من الراكب
            reference: passengerReference,
            description: $"Trip {trip.Id} fare paid from wallet"
        );

        await _walletTransactionRepository.AddAsync(passengerTx, cancellationToken);
        await _walletTransactionRepository.SaveChangesAsync(cancellationToken);

        // 7) نسجّل Payment كـ Completed (بدون Stripe)
        var payment = trip.Payment;
        if (payment == null)
        {
            payment = new Payment(
                tripId: trip.Id,
                paymentMethodId: trip.PaymentMethodId,
                amount: amountToCharge,
                transactionReference: passengerReference,
                status: PaymentStatus.Completed,
                currency: trip.Currency
            );

            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _paymentRepository.SaveChangesAsync(cancellationToken);

            trip.PaymentId = payment.Id;
        }
        else
        {
            payment.Status = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;
            payment.UpdateAmount(amountToCharge);
            await _paymentRepository.UpdateAsync(payment, cancellationToken);
            await _paymentRepository.SaveChangesAsync(cancellationToken);
        }

        // 8) نكمّل حسابات السواق زي ConfirmStripeTripPaymentCommand
        decimal serviceFeePercent =
            await _serviceFeeService.GetApplicableServiceFeePercentAsync(trip.DriverId, cancellationToken);

        var tips = trip.Tips;
        decimal appFee = fareBeforeDiscount * (serviceFeePercent / 100m);
        decimal driverEarning = fareBeforeDiscount - appFee + tips;
        if (driverEarning < 0) driverEarning = 0;

        _logger.LogInformation(
            "WalletTripPayment: Trip {TripId} - Fare {Fare}, Tips {Tips}, ServiceFee {ServiceFeePercent}%, AppFee {AppFee}, DriverEarning {DriverEarning}",
            trip.Id, fareBeforeDiscount, tips, serviceFeePercent, appFee, driverEarning);

        // Earning
        var existingEarning = await _earningRepository
            .Get(e => e.TripId == trip.Id && e.PaymentId == payment.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingEarning == null)
        {
            var earning = new Earning(
                driverId: trip.DriverId,
                tripId: trip.Id,
                amount: driverEarning,
                paymentId: payment.Id
            );

            await _earningRepository.AddAsync(earning, cancellationToken);
            await _earningRepository.SaveChangesAsync(cancellationToken);
        }

        // Driver wallet
        var driverUserId = await _appUserService.GetUserIdByDriverIdAsync(trip.DriverId);
        if (driverUserId != null && driverUserId != Guid.Empty)
        {
            var driverWallet = await _walletRepository
                .Get(w => w.UserId == driverUserId.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (driverWallet == null)
            {
                driverWallet = new Wallet(driverUserId.Value);
                await _walletRepository.AddAsync(driverWallet, cancellationToken);
                await _walletRepository.SaveChangesAsync(cancellationToken);
            }

            var refDriver = $"TRIPWALLETNET-{trip.Id.ToString("N").Substring(0, 12).ToUpper()}";

            var existingDriverTx = await _walletTransactionRepository
                .Get(t => t.WalletId == driverWallet.Id && t.Reference == refDriver)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingDriverTx == null)
            {
                driverWallet.Credit(driverEarning);
                await _walletRepository.UpdateAsync(driverWallet, cancellationToken);

                var driverTx = new WalletTransaction(
                    walletId: driverWallet.Id,
                    amount: driverEarning,
                    type: TransactionType.Credit,
                    transactionCategory: WalletTransactionCategory.TripOnlineNet,
                    reference: refDriver,
                    description: $"Wallet trip earning for trip {trip.Id}"
                );

                await _walletTransactionRepository.AddAsync(driverTx, cancellationToken);
                await _walletTransactionRepository.SaveChangesAsync(cancellationToken);
            }
        }

        // 9) نحدّث حالة الرحلة + IsPaid
        trip.IsPaid = true;
        trip.CompleteTrip();

        await _tripRepository.UpdateAsync(trip, cancellationToken);
        await _tripRepository.SaveChangesAsync(cancellationToken);


        #region Notifications

        // ---------------- Passenger ----------------
        if (passengerUserId != null && passengerUserId != Guid.Empty)
        {
            await _notifier.SendToUserAsync(
                passengerUserId.Value,
                "تم الدفع من المحفظة",
                $"تم خصم {amountToCharge} من محفظتك لرحلتك رقم {trip.Id}.",
                new { tripId = trip.Id }
            );

            await _mediator.Send(new AddNotificationsCommand(
                passengerUserId.Value,
                "تم الدفع من المحفظة",
                $"تم خصم {amountToCharge} من محفظتك لرحلتك رقم {trip.Id}.",
                NotificationType.PaymentSuccess
            ));
        }
        else
        {
            _logger.LogWarning(
                "No ApplicationUser found for PassengerId {PassengerId} in Trip {TripId}",
                trip.PassengerId,
                trip.Id
            );
        }

        // ---------------- Driver ----------------
        if (driverUserId != null && driverUserId != Guid.Empty)
        {
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
        }
        else
        {
            _logger.LogWarning(
                "No ApplicationUser found for DriverId {DriverId} in Trip {TripId}",
                trip.DriverId,
                trip.Id
            );
        }

        #endregion


        //// 🔔 Notifications
        //await _notifier.SendToUserAsync(
        //    trip.PassengerId,
        //    "تم الدفع من المحفظة",
        //    $"تم خصم {amountToCharge} من محفظتك لرحلتك رقم {trip.Id}.",
        //    new { tripId = trip.Id });

        //await _mediator.Send(new AddNotificationsCommand(
        //    trip.PassengerId,
        //    "تم الدفع من المحفظة",
        //    $"تم خصم {amountToCharge} من محفظتك لرحلتك رقم {trip.Id}.",
        //    NotificationType.PaymentSuccess));



        //await _notifier.SendToUserAsync(
        //    trip.DriverId,
        //    "دخل جديد",
        //    $"تم إضافة {driverEarning} جنيه إلى محفظتك.",
        //    new { tripId = trip.Id, earning = driverEarning });

        //await _mediator.Send(new AddNotificationsCommand(
        //    trip.DriverId,
        //    "دخل جديد",
        //    $"تم إضافة {driverEarning} جنيه إلى محفظتك.",
        //    NotificationType.DriverEarning));
        
        return Result<bool>.Success(true);
    }
}
