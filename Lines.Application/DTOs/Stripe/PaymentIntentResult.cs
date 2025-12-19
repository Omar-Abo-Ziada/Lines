namespace Lines.Application.Interfaces.Stripe
{
    public class PaymentIntentResult
    {
        // هل العملية نجحت ولا لأ
        public bool Success { get; set; }
        // status من Stripe (succeeded, requires_action, canceled, ...)
        public string? Status { get; set; }

        // الـ Id بتاع الـ PaymentIntent من Stripe (pi_xxx)
        public string? PaymentIntentId { get; set; }

        // الـ client_secret اللي هنبعته للموبايل
        public string? ClientSecret { get; set; }

        // المبلغ بعد ما نحوله من cents لـ decimal
        public decimal Amount { get; set; }

        public string? Currency { get; set; }

        // لو Stripe رجّعت error
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
