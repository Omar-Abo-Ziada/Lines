using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Models.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Wallets.TopUpOnline.Commands
{
    // بيشحن المحفظة عن طريق إنشاء Payment Intent في Stripe
    public record CreateWalletTopUpPaymentIntentCommand(
        Guid UserId,
        decimal Amount,
        string Currency
    ) : IRequest<Result<CreateWalletTopUpPaymentIntentDto>>;

    public class CreateWalletTopUpPaymentIntentCommandHandler
        : RequestHandlerBase<CreateWalletTopUpPaymentIntentCommand, Result<CreateWalletTopUpPaymentIntentDto>>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly IRepository<WalletTopUp> _topUpRepository;

        public CreateWalletTopUpPaymentIntentCommandHandler(
            RequestHandlerBaseParameters parameters,
            IPaymentGateway paymentGateway,
            IRepository<WalletTopUp> topUpRepository)
            : base(parameters)
        {
            _paymentGateway = paymentGateway;
            _topUpRepository = topUpRepository;
        }

        public override async Task<Result<CreateWalletTopUpPaymentIntentDto>> Handle(
            CreateWalletTopUpPaymentIntentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
            {
                return Result<CreateWalletTopUpPaymentIntentDto>.Failure(
                    Error.Create("WalletTopUp.InvalidAmount", "Amount must be greater than zero."));
            }

            var currency = string.IsNullOrWhiteSpace(request.Currency)
                ? "egp"
                : request.Currency.ToLowerInvariant();

            // Stripe عادة تتعامل بـ minor units (قرش/cent)
            var amountInMinorUnit = (long)(request.Amount * 100m);

            var description = $"Wallet top-up for user {request.UserId} amount {request.Amount} {currency}";

            var paymentIntent = await _paymentGateway.CreatePaymentIntentAsync(
                amountInMinorUnit,
                currency,
                description);

            // نسجّل الـ TopUp في الداتا بيز عشان:
            // - نعرف نعمل Idempotency
            // - نستخدمه في الـ webhook
            //var topUp = new WalletTopUp
            //{
            //    UserId = request.UserId,
            //    Amount = request.Amount,
            //    Currency = currency,
            //    PaymentIntentId = paymentIntent.PaymentIntentId,
            //    Status = WalletTopUpStatus.Pending
            //};
            var topUp = new WalletTopUp(
                request.UserId,
                request.Amount,
                currency,
                paymentIntent.PaymentIntentId
            );


            await _topUpRepository.AddAsync(topUp, cancellationToken);
            await _topUpRepository.SaveChangesAsync(cancellationToken);

            var dto = new CreateWalletTopUpPaymentIntentDto
            {
                PaymentIntentId = paymentIntent.PaymentIntentId,
                ClientSecret = paymentIntent.ClientSecret,
                //AmountInMinorUnit = paymentIntent.Amount,
                Currency = paymentIntent.Currency
            };

            return Result<CreateWalletTopUpPaymentIntentDto>.Success(dto);
        }
    }

}
