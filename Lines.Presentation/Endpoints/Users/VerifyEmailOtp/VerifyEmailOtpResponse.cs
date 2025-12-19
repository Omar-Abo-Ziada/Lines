namespace Lines.Presentation.Endpoints.Users.VerifyEmailOtp;

public class VerifyEmailOtpResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
}
