using Lines.Domain.Enums;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Drivers;

public class WalletTransaction : BaseModel
{
    public Guid WalletId { get; set; }
    public virtual Wallet Wallet { get; set; }
    
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public WalletTransactionCategory TransactionCategory { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }

    // Constructor
    public WalletTransaction(Guid walletId, decimal amount, TransactionType type, 
        WalletTransactionCategory transactionCategory, string reference, string description = null)
    {
        if (walletId == Guid.Empty)
            throw new ArgumentException("WalletId must be valid.", nameof(walletId));
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Reference is required.", nameof(reference));

        WalletId = walletId;
        Amount = amount;
        Type = type;
        TransactionCategory = transactionCategory;
        Reference = reference;
        Description = description ?? string.Empty;
    }

    // Just for data seeding
    public WalletTransaction()
    {
    }
}

