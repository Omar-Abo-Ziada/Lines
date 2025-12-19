using Lines.Domain.Models.Chats;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Passengers;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Models.User;
using Lines.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace Lines.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid> 
{
    public bool IsActive { get; set; } 
    public bool IsDeleted { get; set; }
    public Passenger? Passenger { get; set; }
    public Guid? PassengerId { get; set; }
    public Driver? Driver { get; set; }
    public Guid? DriverId { get; set; }
    public string? StripeCustomerId { get; set; }
    public virtual Wallet Wallet { get; set; }   
    public virtual ICollection<Otp>? Otps { get; set; }  
    public virtual ICollection<Notification>? Notifications { get; set; }
    public virtual ICollection<Activity>? Activities { get; set; } // Added for tracking user activities
    public virtual ICollection<PaymentMethod>? UserPaymentMethods { get; set; }   
    public virtual ICollection<Message>? MessagesSent { get; set; }
    public virtual ICollection<Message>? MessagesRecieved { get; set; }
    public virtual ICollection<EmergencyNumber>? EmergencyNumbers { get; set; }
    public virtual ICollection<UserReward>? UserRewards { get; set; }


    public void setStripeCustomerId(string stripeCustomerId)
    {
        StripeCustomerId = stripeCustomerId;
    }
}