using System;
using System.ComponentModel.DataAnnotations.Schema;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Enums;

namespace Lines.Domain.Models.Trips
{
    //[Table("Payments", Schema = "Trips")]
    public class Payment : BaseModel
    { 
        public decimal Amount { get;  set; }
        public DateTime PaidAt { get;  set; }
        public string TransactionReference { get;  set; }  // to link with the external payment system "like identifier"
        public PaymentStatus Status { get; set; } = PaymentStatus.NotPaidYet;
        public string Currency { get; set; } = "QAR";
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; }
        public Guid PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public Guid? EarningId { get; set; }
        public virtual Earning? Earning { get; set; } // nullable as payment may happen from user to system but earning for driver may not happen yet
       

        #region for stripe
        // 🔹 معرف الـ PaymentIntent في Stripe
        // هو الـ ID الأساسي لكل عملية دفع (pi_xxx)
        // بنستخدمه عشان:
        // 1) نعرف حالة الدفع (succeeded / failed / requires_action)
        // 2) نعمل track لأي retry
        // 3) نجيب بيانات العملية من Stripe لو حصل dispute أو مشكلة
        public string? StripePaymentIntentId { get; set; }

        // 🔹 معرف الـ Charge النهائي في Stripe
        // بعض العمليات (خصوصاً البطاقات) بتنتج Charge (ch_xxx)
        // والمفيد في:
        // - عمليات الـ Refund
        // - تتبع المبالغ المسحوبة فعليًا
        // - reconciliation مع التقارير الشهرية
        public string? StripeChargeId { get; set; }
        #endregion


        // Constructor
        public Payment(Guid tripId, Guid paymentMethodId, decimal amount, string transactionReference, PaymentStatus status = PaymentStatus.NotPaidYet, string currency = "QAR")
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("tripId must be valid.", nameof(tripId));
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive.", nameof(amount));
            if (string.IsNullOrWhiteSpace(transactionReference))
                throw new ArgumentException("Transaction reference cannot be empty.", nameof(transactionReference));

            TripId = tripId;
            PaymentMethodId = paymentMethodId;
            Amount = amount;
            PaidAt = DateTime.UtcNow;
            TransactionReference = transactionReference;
            Status = status;
            Currency = currency;
        }


        // Just for data seeding
        public Payment()
        {

        }
        // Business Methods

        public void UpdateTransactionReference(string newReference)
        {
            if (string.IsNullOrWhiteSpace(newReference))
                throw new ArgumentException("Transaction reference cannot be empty.", nameof(newReference));
            TransactionReference = newReference;
        }

        public void UpdateAmount(decimal newAmount)
        {
            if (newAmount <= 0)
                throw new ArgumentException("Amount must be positive.", nameof(newAmount));
            Amount = newAmount;
        }
    }
}