using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Drivers.BankAccounts.UpdateBankAccount.Commands;

public record UpdateBankAccountCommand(Guid DriverId, Guid BankAccountId, UpdateBankAccountDto BankAccountData) : IRequest<Result<BankAccountDto>>;

public class UpdateBankAccountCommandHandler : RequestHandlerBase<UpdateBankAccountCommand, Result<BankAccountDto>>
{
    private readonly IRepository<BankAccount> _bankAccountRepository;

    public UpdateBankAccountCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<BankAccount> bankAccountRepository) : base(parameters)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public override async Task<Result<BankAccountDto>> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
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

            // Update bank account details
            bankAccount.UpdateBankDetails(
                request.BankAccountData.BankName,
                request.BankAccountData.IBAN,
                request.BankAccountData.SWIFT,
                request.BankAccountData.AccountHolderName,
                request.BankAccountData.AccountNumber,
                request.BankAccountData.BranchName,
                request.BankAccountData.CardNumber,
                request.BankAccountData.ExpiryDate,
                request.BankAccountData.CVV
            );
            
            // Update IsPrimary if changed
            if (request.BankAccountData.IsPrimary)
            {
                bankAccount.SetAsPrimary();
            }
            else
            {
                bankAccount.SetAsSecondary();
            }

            await _bankAccountRepository.UpdateAsync(bankAccount, cancellationToken);
            _bankAccountRepository.SaveChanges();

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
        catch (ArgumentException ex)
        {
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("400", ex.Message, Application.Shared.ErrorType.Validation));
        }
        catch (Exception ex)
        {
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("500", $"Error updating bank account: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

