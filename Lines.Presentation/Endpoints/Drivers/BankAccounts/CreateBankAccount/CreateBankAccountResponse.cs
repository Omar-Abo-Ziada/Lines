namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.CreateBankAccount;

public class CreateBankAccountResponse
{
    public Guid Id { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

