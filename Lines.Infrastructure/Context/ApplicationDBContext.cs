using Lines.Application.Interfaces;
using Lines.Domain.Models;
using Lines.Domain.Models.Chats;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Drivers.StripeAccounts;
using Lines.Domain.Models.License;
using Lines.Domain.Models.Notifications;
using Lines.Domain.Models.Passengers;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Models.Sites;
using Lines.Domain.Models.TermsAndConditions;
using Lines.Domain.Models.Trips;
using Lines.Domain.Models.User;
using Lines.Domain.Models.Users;
using Lines.Domain.Models.Vehicles;
using Lines.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Activity = Lines.Domain.Models.Users.Activity;


namespace Lines.Infrastructure.Context;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDBContext
{
    public ApplicationDBContext() : base() { }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Example> Examples { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<License> Licenses { get; set; }
    public DbSet<Earning> Earnings { get; set; }
    public DbSet<EmergencyNumber> EmergencyNumbers { get; set; }
    public DbSet<EndTripLocation> EndTripLocations { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<FavoriteLocation> FavoriteLocations { get; set; }  // here >> test fav locations end point
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<DriverServiceFeeOffer> DriverServiceFeeOffers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<TripRequest> TripRequests { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehiclePhoto> VehiclePhotos { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<DriverRegistration> DriverRegistrations { get; set; }
    public DbSet<DriverAddress> DriverAddresses { get; set; }
    public DbSet<DriverOfferActivation> DriverOfferActivations { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<DriverBlockedPassenger> DriverBlockedPassengers { get; set; }
    public DbSet<FCMUserToken> FCMUserTokens { get; set; }
    public DbSet<DriverEmergencyContact> DriverEmergencyContacts { get; set; }
    public DbSet<WalletTopUp> WalletTopUps { get; set; }
    public DbSet<DriverStripeAccount> DriverStripeAccounts { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // This will apply all IEntityTypeConfiguration<T> found in the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);



    }

}
