using Lines.Application.Features.Drivers.BankAccounts.DTOs;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Drivers.BankAccounts.CreateBankAccount.Commands;

public record CreateBankAccountCommand(Guid DriverId, CreateBankAccountDto BankAccountData) : IRequest<Result<BankAccountDto>>;

public class CreateBankAccountCommandHandler : RequestHandlerBase<CreateBankAccountCommand, Result<BankAccountDto>>
{
    private readonly IRepository<BankAccount> _bankAccountRepository;

    public CreateBankAccountCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<BankAccount> bankAccountRepository) : base(parameters)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public override async Task<Result<BankAccountDto>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var bankAccount = new BankAccount(
                request.BankAccountData.BankName,
                request.BankAccountData.IBAN,
                request.BankAccountData.SWIFT,
                request.BankAccountData.AccountHolderName,
                request.DriverId,
                request.BankAccountData.AccountNumber,
                request.BankAccountData.BranchName,
                request.BankAccountData.IsPrimary,
                request.BankAccountData.CardNumber,
                request.BankAccountData.ExpiryDate,
                request.BankAccountData.CVV
            );

            await _bankAccountRepository.AddAsync(bankAccount, cancellationToken);
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
            return Result<BankAccountDto>.Failure(new Application.Shared.Error("500", $"Error creating bank account: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}

