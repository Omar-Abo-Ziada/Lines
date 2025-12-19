using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Drivers.BankAccounts.GetBankAccountById.Queries;

public record GetBankAccountByIdQuery(Guid DriverId, Guid BankAccountId) : IRequest<Result<BankAccountDto>>;

public class GetBankAccountByIdQueryHandler : RequestHandlerBase<GetBankAccountByIdQuery, Result<BankAccountDto>>
{
    private readonly IRepository<BankAccount> _bankAccountRepository;

    public GetBankAccountByIdQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<BankAccount> bankAccountRepository) : base(parameters)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public override async Task<Result<BankAccountDto>> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

            if (bankAccount == null)
            {
                return Result<BankAccountDto>.Failure(new Application.Shared.Error("404", "Bank account not found", Application.Shared.ErrorType.NotFound));
            }

            // Verify ownership
            if (bankAccount.DriverId != request.DriverId)
            {
                return Result<BankAccountDto>.Failure(new Application.Shared.Error("403", "Access denied. Bank account does not belong to this driver", Application.Shared.ErrorType.Validation));
            }

            var dto = new BankAccountDto
            {
                Id = bankAccount.Id,
                BankName = bankAccount.BankName,
                IBAN = bankAccount.IBAN,
                SWIFT = bankAccount.SWIFT,
                AccountHolderName = bankAccount.AccountHolderName,
                AccountNumber = bankAccount.AccountNumber,
                BranchName = bankAccount.BranchName,
                IsPrimary = bankAccount.IsPrimary,
                CardNumber = bankAccount.CardNumber,
                ExpiryDate = bankAccount.ExpiryDate,
                CVV = bankAccount.CVV,
                DriverId = bankAccount.DriverId,
                CreatedDate = bankAccount.CreatedDate,
                UpdatedDate = bankAccount.UpdatedDate
            };

            return Result<BankAccountDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("500", $"Error retrieving bank account: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

