using Lines.Domain.Enums;
using Lines.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Models.Drivers
{
 
    public class WalletTopUp : BaseModel
    {
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "QAR";
        public string PaymentIntentId { get; private set; } = null!;
        public WalletTopUpStatus Status { get; private set; }
        public string? FailureReason { get; private set; }
        public Guid? WalletTransactionId { get; private set; }



        public WalletTopUp() { }
        //protected WalletTopUp() { }

        public WalletTopUp(Guid userId, decimal amount, string currency, string paymentIntentId)
        {
            UserId = userId;
            Amount = amount;
            Currency = currency;
            PaymentIntentId = paymentIntentId;
           Status = WalletTopUpStatus.Pending;
        }

        public void MarkSucceeded(Guid? walletTransactionId = null)
        {
            Status = WalletTopUpStatus.Succeeded;
            WalletTransactionId = walletTransactionId;
            FailureReason = null;
        }

        //public void MarkSucceeded(Guid walletTransactionId)
        //{
        //    if (Status != WalletTopUpStatus.Pending)
        //        throw new InvalidOperationException("Top-up already processed.");

        //    Status = WalletTopUpStatus.Succeeded;
        //    WalletTransactionId = walletTransactionId;
        //}

        public void MarkFailed(string reason)
        {
            if (Status != WalletTopUpStatus.Pending)
                return;

            Status = WalletTopUpStatus.Failed;
            FailureReason = reason;
        }
        public void MarkPending(string? reason = null)
        {
            Status = WalletTopUpStatus.Pending;
            FailureReason = reason;
        }

    }

}
