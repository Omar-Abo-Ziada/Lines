namespace Lines.Presentation.Endpoints.Wallets.GetWalletTransactions;

public class GetWalletTransactionsResponse
{
    public List<WalletTransactionItem> Transactions { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class WalletTransactionItem
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string TransactionCategory { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

