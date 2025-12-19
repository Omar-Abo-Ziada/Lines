// using Lines.Domain.Enums;
// using Lines.Domain.Models.Common;
// using Lines.Domain.Models.Common;
// using Lines.Domain.Models.PaymentMethods;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace Lines.Domain.Models.Users
// {
//     public class UserPaymentMethod  : BaseModel ///TODO: split it into separate tables each one represent specific payment method type and its users so can enforce the required data in constructor??????
//     {
//
//         public bool IsDefault { get; set; }  // if registered default payment method should be apple pay , else should be cash, each user can change and have dif default payment method
//
//         // Credit Card
//         public string? CardHolderName { get; set; }
//         public string? CardNumberMasked { get; set; }
//         public string? CardBrand { get; set; }
//         public string? CardExpiryMonth { get; set; }
//         public string? CardExpiryYear { get; set; }
//         public string? CardLast4 { get; set; }
//
//         // PayPal
//         public string? PayPalEmail { get; set; }
//
//         // Apple Pay & Google Pay (tokenized, usually not stored, but for reference)
//         public string? TokenReference { get; set; }
//
//         // Cash (no extra fields, but you may want a flag)
//         public bool IsCash { get; set; }
//         public Guid UserId { get; set; }
//         public virtual User User { get; set; }
//
//         public int PaymentMethodId { get; set; }
//         public virtual BasePaymentMethod PaymentMethod { get; set; }
//
//
//         public UserPaymentMethod(Guid userId , PaymentMethodType paymentMethodType, bool isDefault = false)      
//         {
//             PaymentMethodId = (int)paymentMethodType; // Assuming PaymentMethodType is an enum that maps to the ID in the database
//         }
//
//
//         // Just for data seeding
//         public UserPaymentMethod()
//         {
//
//         }
//     }
// }
