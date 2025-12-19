using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TopUpWallet.Commands;
using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Models.Drivers;


namespace Lines.Application.Features.Wallets.TopUpOnline.Commands;

public record ConfirmWalletTopUpCommand(
    Guid UserId,
    string PaymentIntentId
) : IRequest<Result<TopUpWalletDto>>;

public class ConfirmWalletTopUpCommandHandler
    : RequestHandlerBase<ConfirmWalletTopUpCommand, Result<TopUpWalletDto>>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IRepository<WalletTopUp> _topUpRepository;

    public ConfirmWalletTopUpCommandHandler(
        RequestHandlerBaseParameters parameters,
        IPaymentGateway paymentGateway,
        IRepository<WalletTopUp> topUpRepository)
        : base(parameters)
    {
        _paymentGateway = paymentGateway;
        _topUpRepository = topUpRepository;
    }

    public override async Task<Result<TopUpWalletDto>> Handle(
        ConfirmWalletTopUpCommand request,
        CancellationToken cancellationToken)
    {
        // نجيب الـ TopUp اللي اتعمل له PaymentIntent
        var topUp = await _topUpRepository
            .Get(t => t.PaymentIntentId == request.PaymentIntentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (topUp == null)
        {
            return Result<TopUpWalletDto>.Failure(
                Error.Create("WalletTopUp.NotFound", "No wallet top-up found for this payment intent."));
        }

        if (topUp.UserId != request.UserId)
        {
            return Result<TopUpWalletDto>.Failure(
                Error.Create("WalletTopUp.Unauthorized", "This top-up does not belong to the current user."));
        }

        // Idempotency
        if (topUp.Status == WalletTopUpStatus.Succeeded)
        {
            // نظريًا هنا ممكن ترجع آخر state من الـ Wallet مباشرة، بس علشان البساطة
            // هنرجّع Failure بسيطة تقول إن العملية تمت قبل كده
            return Result<TopUpWalletDto>.Failure(
                Error.Create("WalletTopUp.AlreadySucceeded", "Top-up already processed."));
        }

        // نسأل Stripe على حالة الـ PaymentIntent
        var stripeResult = await _paymentGateway.ConfirmPaymentAsync(request.PaymentIntentId);

        if (!stripeResult.Success && stripeResult.RequiresAction)
        {
            //topUp.Status = WalletTopUpStatus.Pending;
            //topUp.FailureReason = "Requires additional authentication (3D Secure).";
            topUp.MarkPending("Requires additional authentication (3D Secure).");
            await _topUpRepository.UpdateAsync(topUp, cancellationToken);
            await _topUpRepository.SaveChangesAsync(cancellationToken);

            return Result<TopUpWalletDto>.Failure(
                Error.Create("Payment.RequiresAction", topUp.FailureReason));
        }

        if (!stripeResult.Success ||
            !string.Equals(stripeResult.Status, "succeeded", StringComparison.OrdinalIgnoreCase))
        {
            topUp.MarkFailed(stripeResult.ErrorMessage ?? "Payment did not succeed.");
            await _topUpRepository.UpdateAsync(topUp, cancellationToken);
            await _topUpRepository.SaveChangesAsync(cancellationToken);


            return Result<TopUpWalletDto>.Failure(
                Error.Create("Payment.NotSucceeded", topUp.FailureReason));
        }

        // كده الدفع نجح → نستخدم TopUpWalletCommand (اللي عندك) عشان نعمل credit
        var result = await _mediator.Send(
            new TopUpWalletCommand(topUp.UserId, topUp.Amount),
            cancellationToken);

        if (!result.IsSuccess)
        {
            // لو حصلت مشكلة في الـ Wallet بنسجّلها
            //topUp.Status = WalletTopUpStatus.Failed;
            topUp.MarkFailed(result.Error.Description);
            //topUp.FailureReason = result.Error.Description;
            await _topUpRepository.UpdateAsync(topUp, cancellationToken);
            await _topUpRepository.SaveChangesAsync(cancellationToken);

            return result;
        }

        // تم بنجاح
        //topUp.Status = WalletTopUpStatus.Succeeded;
        topUp.MarkSucceeded();

        //topUp.FailureReason = null;
        // ممكن تربط TransactionId لو تحب تعدّل DTO/Command ترجّع Id المعاملة
        await _topUpRepository.UpdateAsync(topUp, cancellationToken);
        await _topUpRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}

