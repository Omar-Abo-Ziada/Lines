namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.PersonalInfo;

public class RegisterDriverPersonalInfoResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string SessionToken { get; set; } = string.Empty;
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; } = 6;
}
