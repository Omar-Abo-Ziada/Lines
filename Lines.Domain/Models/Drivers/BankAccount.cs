using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Drivers;

public class BankAccount : BaseModel
{
    public string BankName { get; set; }
    public string IBAN { get; set; }
    public string SWIFT { get; set; }
    public string AccountHolderName { get; set; }
    
    // New optional fields
    public string? AccountNumber { get; set; }
    public string? BranchName { get; set; }
    public bool IsPrimary { get; set; } = false;
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? CVV { get; set; }
    
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }

    public BankAccount(string bankName, string iban, string swift, string accountHolderName, Guid driverId,
        string? accountNumber = null, string? branchName = null, bool isPrimary = false,
        string? cardNumber = null, string? expiryDate = null, string? cvv = null)
    {
        ValidateBankAccountDetails(bankName, iban, swift, accountHolderName, driverId);
        
        BankName = bankName;
        IBAN = iban;
        SWIFT = swift;
        AccountHolderName = accountHolderName;
        AccountNumber = accountNumber;
        BranchName = branchName;
        IsPrimary = isPrimary;
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
        CVV = cvv;
        DriverId = driverId;
    }

    // For data seeding
    public BankAccount()
    {
    }

    private void ValidateBankAccountDetails(string bankName, string iban, string swift, string accountHolderName, Guid driverId)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("Bank name cannot be empty", nameof(bankName));
        
        if (string.IsNullOrWhiteSpace(iban))
            throw new ArgumentException("IBAN cannot be empty", nameof(iban));
        
        if (string.IsNullOrWhiteSpace(swift))
            throw new ArgumentException("SWIFT code cannot be empty", nameof(swift));
        
        if (string.IsNullOrWhiteSpace(accountHolderName))
            throw new ArgumentException("Account holder name cannot be empty", nameof(accountHolderName));
        
        if (driverId == Guid.Empty)
            throw new ArgumentException("Driver ID cannot be empty", nameof(driverId));
    }

    public void UpdateBankDetails(string bankName, string iban, string swift, string accountHolderName,
        string? accountNumber = null, string? branchName = null, string? cardNumber = null, string? expiryDate = null, string? cvv = null)
    {
        ValidateBankAccountDetails(bankName, iban, swift, accountHolderName, DriverId);
        
        BankName = bankName;
        IBAN = iban;
        SWIFT = swift;
        AccountHolderName = accountHolderName;
        AccountNumber = accountNumber;
        BranchName = branchName;
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
        CVV = cvv;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void SetAsSecondary()
    {
        IsPrimary = false;
    }
}
