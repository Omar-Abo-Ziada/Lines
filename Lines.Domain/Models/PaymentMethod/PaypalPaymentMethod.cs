// using System.ComponentModel.DataAnnotations.Schema;
// using Lines.Domain.Enums;
//
// namespace Lines.Domain.Models.PaymentMethods
// {
//     //[Table("CreditCardPaymentMethods", Schema = "PaymentMethod")]
//     public class PaypalPaymentMethod : BasePaymentMethod
//     {
//         public string Email { get; private set; }
//
//         public PaypalPaymentMethod(string email)
//             : base(PaymentMethodType.Paypal)
//         {
//             if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
//                 throw new ArgumentException("Invalid email address");
//
//             Email = email;
//         }
//
//         private bool IsValidEmail(string email)
//         {
//             try
//             {
//                 var addr = new System.Net.Mail.MailAddress(email);  // no problem as it is a built in .net 
//                 return addr.Address == email;
//             }
//             catch
//             {
//                 return false;
//             }
//         }
//
//         public override bool IsExpired() => false; // PayPal accounts don't expire
//     }
// }