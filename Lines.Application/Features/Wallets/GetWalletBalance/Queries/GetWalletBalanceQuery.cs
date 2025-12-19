using Lines.Application.Common;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Wallets.GetWalletBalance.Queries;

public record GetWalletBalanceQuery(Guid UserId) : IRequest<Result<WalletBalanceDto>>;

public class GetWalletBalanceQueryHandler : RequestHandlerBase<GetWalletBalanceQuery, Result<WalletBalanceDto>>
{
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IApplicationUserService _appUserService;

    public GetWalletBalanceQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Wallet> walletRepository,
        IApplicationUserService appUserService)
        : base(parameters)
    {
        _walletRepository = walletRepository;
        _appUserService = appUserService;
    }

    public override async Task<Result<WalletBalanceDto>> Handle(
        GetWalletBalanceQuery request,
        CancellationToken cancellationToken)
    {

      

        // Get or create wallet
        var wallet = await _walletRepository
            .Get(w => w.UserId == request.UserId)
            //.Get(w => w.DriverId == request.DriverId)
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync(cancellationToken);

        if (wallet == null)
        {
            // Create wallet for driver
            wallet = new Wallet(request.UserId);
            await _walletRepository.AddAsync(wallet, cancellationToken);
            await _walletRepository.SaveChangesAsync(cancellationToken);
        }

        var recentTransactions = wallet.Transactions
            .OrderByDescending(t => t.CreatedDate)
            .Take(10)
            .Select(t => new RecentTransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type.ToString(),
                Category = t.TransactionCategory.ToString(),
                Description = t.Description,
                CreatedDate = t.CreatedDate
            })
            .ToList();

        var dto = new WalletBalanceDto
        {
            WalletId = wallet.Id,
            Balance = wallet.Balance,
            LastUpdated = wallet.UpdatedDate ?? wallet.CreatedDate,
            RecentTransactions = recentTransactions
        };

        return Result<WalletBalanceDto>.Success(dto);
    }
}

