namespace Lines.Presentation.Endpoints.Drivers.UpdateDriverContact;

public class UpdateDriverContactResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public string UpdatedFields { get; set; } = string.Empty;
}
