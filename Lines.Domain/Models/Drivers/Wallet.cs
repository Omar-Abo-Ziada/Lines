using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Drivers;

//this wallet is for driver to manage his earnings from trips and for passenger 
public class Wallet : BaseModel
{
    //public Guid DriverId { get; set; }
    //public virtual Driver Driver { get; set; }
    public Guid UserId { get; set; }   // ApplicationUser Id, UserId for driver or passenger

    public decimal Balance { get; private set; }
    
    public virtual ICollection<WalletTransaction> Transactions { get; set; }

    // Constructor
    public Wallet(Guid userId, decimal initialBalance = 0) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId must be valid.", nameof(userId));
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

        UserId = userId;
        //DriverId = driverId;
        Balance = initialBalance;
        Transactions = new List<WalletTransaction>();
    }

    // Just for data seeding
    public Wallet()
    {
        Transactions = new List<WalletTransaction>();
    }

    // Business Methods
    public bool HasSufficientBalance(decimal amount)
    {
        return Balance >= amount;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Credit amount must be positive.", nameof(amount));

        Balance += amount;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Debit amount must be positive.", nameof(amount));
        if (!HasSufficientBalance(amount))
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
    }
}

