// using System.ComponentModel.DataAnnotations.Schema;
// using Lines.Domain.Enums;
//
// namespace Lines.Domain.Models.PaymentMethods
// {
//     //[Table("CreditCardPaymentMethods", Schema = "PaymentMethod")]
//     public class CreditCardPaymentMethod : BasePaymentMethod
//     {
//         public string CardNumber { get; private set; }
//         public DateTime ExpiryDate { get; private set; }
//         public string CVV { get; private set; }
//         public string CardHolderName { get; private set; }
//
//         public CreditCardPaymentMethod(string cardNumber, DateTime expiryDate,
//             string cvv, string cardHolderName, string userId)
//             : base(PaymentMethodType.CreditCard)
//         {
//             ValidatePaymentMethod(cardNumber, expiryDate, cvv, cardHolderName);
//             CardNumber = MaskCardNumber(cardNumber);
//             ExpiryDate = expiryDate;
//             CVV = cvv;
//             CardHolderName = cardHolderName;
//         }
//
//         // ... existing code for validation methods ...
//
//         public override bool IsExpired()
//         {
//             return ExpiryDate < DateTime.UtcNow;
//         }
//
//         private void ValidatePaymentMethod(string cardNumber, DateTime expiryDate, string cvv, string cardHolderName)
//         {
//             if (string.IsNullOrWhiteSpace(cardNumber))
//                 throw new ArgumentException("Card number cannot be empty");
//
//             if (!IsValidCardNumber(cardNumber))
//                 throw new ArgumentException("Invalid card number");
//
//             if (expiryDate < DateTime.UtcNow)
//                 throw new ArgumentException("Card has expired");
//
//             if (string.IsNullOrWhiteSpace(cvv) || !IsValidCVV(cvv))
//                 throw new ArgumentException("Invalid CVV");
//
//             if (string.IsNullOrWhiteSpace(cardHolderName))
//                 throw new ArgumentException("Card holder name cannot be empty");
//         }
//
//         private bool IsValidCardNumber(string cardNumber)
//         {
//             // Remove any spaces or hyphens
//             cardNumber = cardNumber.Replace(" ", "").Replace("-", "");
//
//             // Check if the card number contains only digits and has valid length (13-19 digits)
//             if (!cardNumber.All(char.IsDigit) || cardNumber.Length < 13 || cardNumber.Length > 19)
//                 return false;
//
//             // Implement Luhn algorithm for card number validation
//             int sum = 0;
//             bool alternate = false;
//
//             for (int i = cardNumber.Length - 1; i >= 0; i--)
//             {
//                 int digit = cardNumber[i] - '0';
//
//                 if (alternate)
//                 {
//                     digit *= 2;
//                     if (digit > 9)
//                         digit -= 9;
//                 }
//
//                 sum += digit;
//                 alternate = !alternate;
//             }
//
//             return sum % 10 == 0;
//         }
//
//         private bool IsValidCVV(string cvv)
//         {
//             // CVV should be 3 or 4 digits
//             return cvv.All(char.IsDigit) && (cvv.Length == 3 || cvv.Length == 4);
//         }
//
//         private string MaskCardNumber(string cardNumber)
//         {
//             // Keep first 6 and last 4 digits, mask the rest
//             string cleanNumber = cardNumber.Replace(" ", "").Replace("-", "");
//             string masked = cleanNumber.Substring(0, 6) + new string('*', cleanNumber.Length - 10) + cleanNumber.Substring(cleanNumber.Length - 4);
//
//             // Format the masked number in groups of 4
//             return string.Join(" ", masked.Select((c, i) => new { c, i })
//                                        .GroupBy(x => x.i / 4)
//                                        .Select(g => new string(g.Select(x => x.c).ToArray())));
//         }
//
//
//     }
// }