namespace Lines.Application.Features.Wallets.DTOs;

public class WalletTransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string TransactionCategory { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

