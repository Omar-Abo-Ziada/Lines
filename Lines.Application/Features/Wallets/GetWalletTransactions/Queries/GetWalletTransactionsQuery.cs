using Lines.Application.Common;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Wallets.GetWalletTransactions.Queries;

public record GetWalletTransactionsQuery(Guid UserId, int Page, int PageSize) : IRequest<Result<PaginatedWalletTransactionsDto>>;

public class PaginatedWalletTransactionsDto
{
    public List<WalletTransactionDto> Transactions { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class GetWalletTransactionsQueryHandler 
    : RequestHandlerBase<GetWalletTransactionsQuery, Result<PaginatedWalletTransactionsDto>>
{
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IRepository<WalletTransaction> _transactionRepository;

    public GetWalletTransactionsQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Wallet> walletRepository,
        IRepository<WalletTransaction> transactionRepository)
        : base(parameters)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
    }

    public override async Task<Result<PaginatedWalletTransactionsDto>> Handle(
        GetWalletTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        // Get wallet
        var wallet = await _walletRepository
            .Get(w => w.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (wallet == null)
        {
            return Result<PaginatedWalletTransactionsDto>.Failure(new Error(
                Code: "WALLET:NOTFOUND",
                Description: "Wallet not found for this driver.",
                Type: ErrorType.NotFound));
        }

        // Get total count
        var totalCount = await _transactionRepository
            .Get(t => t.WalletId == wallet.Id)
            .CountAsync(cancellationToken);

        // Get paginated transactions
        var transactions = await _transactionRepository
            .Get(t => t.WalletId == wallet.Id)
            .OrderByDescending(t => t.CreatedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new WalletTransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type.ToString(),
                TransactionCategory = t.TransactionCategory.ToString(),
                Reference = t.Reference,
                Description = t.Description,
                CreatedDate = t.CreatedDate
            })
            .ToListAsync(cancellationToken);

        var result = new PaginatedWalletTransactionsDto
        {
            Transactions = transactions,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };

        return Result<PaginatedWalletTransactionsDto>.Success(result);
    }
}

