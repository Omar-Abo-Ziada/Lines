namespace Lines.Application.Interfaces.Stripe
{
    public class PaymentConfirmationResult
    {
        // هل الدفع خلص فعلاً ولا لأ
        public bool Success { get; set; }

        // هل محتاج 3D Secure / action من العميل
        public bool RequiresAction { get; set; }

        public string? PaymentIntentId { get; set; }

        // status من Stripe (succeeded, requires_action, canceled, ...)
        public string? Status { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
