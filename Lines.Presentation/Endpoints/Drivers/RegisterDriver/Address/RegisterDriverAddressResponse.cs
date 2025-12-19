namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Address;

public class RegisterDriverAddressResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; } = 6;
}
