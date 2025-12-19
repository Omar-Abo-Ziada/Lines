namespace Lines.Presentation.Endpoints.Wallets.TopUpWallet;

public class TopUpWalletResponse
{
    public Guid WalletId { get; set; }
    public decimal AmountAdded { get; set; }
    public decimal NewBalance { get; set; }
    public string TransactionReference { get; set; }
    public DateTime TransactionDate { get; set; }
}

