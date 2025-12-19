using Lines.Application.Features.Drivers.BankAccounts.DTOs;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.GetBankAccountById;

public class GetBankAccountByIdResponse
{
    public BankAccountDto BankAccount { get; set; } = new();
}

