using Lines.Application.Features.Drivers.BankAccounts.DTOs;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.GetAllBankAccounts;

public class GetAllBankAccountsResponse
{
    public List<BankAccountDto> BankAccounts { get; set; } = new();
}

