namespace Lines.Presentation.Endpoints.Wallets.GetWallet;

public class GetWalletResponse
{
    public Guid WalletId { get; set; }
    public decimal Balance { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<RecentTransactionResponse> RecentTransactions { get; set; }
}

public class RecentTransactionResponse
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

