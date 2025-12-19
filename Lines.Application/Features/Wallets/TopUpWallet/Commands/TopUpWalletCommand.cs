using Lines.Application.Common;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Wallets.TopUpWallet.Commands;

public record TopUpWalletCommand(Guid UserId, decimal Amount) : IRequest<Result<TopUpWalletDto>>;

public class TopUpWalletCommandHandler : RequestHandlerBase<TopUpWalletCommand, Result<TopUpWalletDto>>
{
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IRepository<WalletTransaction> _transactionRepository;

    public TopUpWalletCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Wallet> walletRepository,
        IRepository<WalletTransaction> transactionRepository)
        : base(parameters)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
    }

    public override async Task<Result<TopUpWalletDto>> Handle(
        TopUpWalletCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Amount <= 0)
            {
                return Result<TopUpWalletDto>.Failure(new Error(
                    Code: "WALLET:INVALIDAMOUNT",
                    Description: "Top-up amount must be greater than zero.",
                    Type: ErrorType.Validation));
            }

            // Get or create wallet
            var wallet = await _walletRepository
                .Get(w => w.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (wallet == null)
            {
                wallet = new Wallet(request.UserId);
                await _walletRepository.AddAsync(wallet, cancellationToken);
                await _walletRepository.SaveChangesAsync(cancellationToken);
            }

            // Credit wallet
            wallet.Credit(request.Amount);
            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            // Create transaction
            var reference = $"TOPUP-{Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()}";
            var transaction = new WalletTransaction(
                walletId: wallet.Id,
                amount: request.Amount,
                type: TransactionType.Credit,
                transactionCategory: WalletTransactionCategory.TopUp,
                reference: reference,
                description: "Wallet top-up"
            );
            
            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _transactionRepository.SaveChangesAsync(cancellationToken);

            var dto = new TopUpWalletDto
            {
                WalletId = wallet.Id,
                AmountAdded = request.Amount,
                NewBalance = wallet.Balance,
                TransactionReference = reference,
                TransactionDate = DateTime.UtcNow
            };

            return Result<TopUpWalletDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<TopUpWalletDto>.Failure(new Error(
                Code: "WALLET:EXCEPTION",
                Description: ex.Message,
                Type: ErrorType.Failure));
        
           }
        // Validate amount
    }
}

