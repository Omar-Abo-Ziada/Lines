namespace Lines.Application.Features.Wallets.DTOs;

public class WalletBalanceDto
{
    public Guid WalletId { get; set; }
    public decimal Balance { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<RecentTransactionDto> RecentTransactions { get; set; }
}

public class RecentTransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

