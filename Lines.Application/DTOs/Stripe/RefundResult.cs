namespace Lines.Application.Interfaces.Stripe
{
    public class RefundResult
    {
        public bool Success { get; set; }

        // Id بتاع الـ Refund نفسه (re_xxx)
        public string? RefundId { get; set; }

        // status (succeeded, pending, failed, ...)
        public string? Status { get; set; }

        // الـ Charge اللي اتعمل عليه refund (ch_xxx)
        public string? ChargeId { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
