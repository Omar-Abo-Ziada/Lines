namespace Lines.Application.Features.Wallets.DTOs;

public class TopUpWalletDto
{
    public Guid WalletId { get; set; }
    public decimal AmountAdded { get; set; }
    public decimal NewBalance { get; set; }
    public string TransactionReference { get; set; }
    public DateTime TransactionDate { get; set; }
}

