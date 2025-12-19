using Lines.Domain.Enums;

namespace Lines.Domain.Models
{
    public interface IPaymentMethod
    {
        Guid Id { get; }
        PaymentMethodType Type { get; }
        bool IsActive { get; }
        DateTime CreatedDate { get; }

        void Deactivate();

        bool IsExpired();
    }
}