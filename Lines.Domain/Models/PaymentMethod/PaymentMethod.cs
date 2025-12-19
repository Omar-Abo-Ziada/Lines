using Lines.Domain.Enums;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.PaymentMethods;
public class PaymentMethod : BaseModel
{
    public PaymentMethodType Type { get; set; }
    public bool IsDefault { get; set; } = false;// Is this the default payment method for the user
    public string CustomerId { get; set; } // Provided by payment gateway
    public string PaymentGatewayPaymentMethodId { get; set; }  // provided by payment gateway
    public Guid UserId { get; set; } // Foreign key to User
    public virtual ICollection<Payment> Payments { get; set; }

    public PaymentMethod(string paymentGatewayPaymentMethodId ,string customerId , Guid userId, PaymentMethodType type, bool isDefault = false)
    {
        Payments = new HashSet<Payment>();
        PaymentGatewayPaymentMethodId = paymentGatewayPaymentMethodId;
        CustomerId = customerId;
        UserId = userId;
        Type = type;
        IsDefault = isDefault;
    }

    public PaymentMethod()
    {
        
    }
    public void SetAsDefault()
    {
        IsDefault = true;
    }

    public void UnsetAsDefault()
    {
        IsDefault = false;
    }

    public void SetPaymentMethodType(PaymentMethodType type)
    {
        Type = type;
    }
}
