using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.BankAccounts.GetAllBankAccounts.Queries;

public record GetAllBankAccountsQuery(Guid DriverId) : IRequest<Result<List<BankAccountDto>>>;

public class GetAllBankAccountsQueryHandler : RequestHandlerBase<GetAllBankAccountsQuery, Result<List<BankAccountDto>>>
{
    private readonly IRepository<BankAccount> _bankAccountRepository;

    public GetAllBankAccountsQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<BankAccount> bankAccountRepository) : base(parameters)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public override async Task<Result<List<BankAccountDto>>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var bankAccounts = await _bankAccountRepository
                .Get(ba => ba.DriverId == request.DriverId && !ba.IsDeleted)
                .OrderBy(ba => ba.CreatedDate)
                .Select(ba => new BankAccountDto
                {
                    Id = ba.Id,
                    BankName = ba.BankName,
                    IBAN = ba.IBAN,
                    SWIFT = ba.SWIFT,
                    AccountHolderName = ba.AccountHolderName,
                    AccountNumber = ba.AccountNumber,
                    BranchName = ba.BranchName,
                    IsPrimary = ba.IsPrimary,
                    CardNumber = ba.CardNumber,
                    ExpiryDate = ba.ExpiryDate,
                    CVV = ba.CVV,
                    DriverId = ba.DriverId,
                    CreatedDate = ba.CreatedDate,
                    UpdatedDate = ba.UpdatedDate
                })
                .ToListAsync(cancellationToken);

            return Result<List<BankAccountDto>>.Success(bankAccounts);
        }
        catch (Exception ex)
        {
            return Result<List<BankAccountDto>>.Failure(new Application.Shared.Error("500", $"Error retrieving bank accounts: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

