namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Withdrawal;

public class RegisterDriverWithdrawalResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid DriverId { get; set; }
    public Guid UserId { get; set; }
    public int CurrentStep { get; set; } = 6;
    public int TotalSteps { get; set; } = 6;
    public bool RegistrationComplete { get; set; } = true;
}
