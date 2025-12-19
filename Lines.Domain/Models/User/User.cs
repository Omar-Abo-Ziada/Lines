using Lines.Domain.Models.Common;
using Lines.Domain.Value_Objects;


namespace Lines.Domain.Models.Users;

public abstract class User : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TotalTrips { get; set; }
    public int RatedTripsCount { get; set; }
    public double Rating { get; set; }
    public int Points { get; set; }
    public Email Email { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
    protected User(Guid id, string firstName, string lastName, Email email, PhoneNumber phoneNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Rating = 5.0; 
        TotalTrips = 0;
        RatedTripsCount = 0;
    }

    public User()
    {
        
    }

    public void UpdateRating(double newRating)
    {
        if (newRating < 1 || newRating > 5)
            throw new ArgumentException("Rating must be between 1 and 5");

        Rating = ((Rating * RatedTripsCount) + newRating) / (RatedTripsCount + 1);          
        RatedTripsCount++;
    }

    public void IncrementTripsCount()
    {
        TotalTrips++;
    }

    public void AddPoints(int points)
    {
       Points += points;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}