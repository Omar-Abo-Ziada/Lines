namespace Lines.Application.Features.Otps.CreateOtp.DTOs
{
    public class CreateOtpDto
    {
        public string Code { get; set; }
        public DateTime OTPGenerationTime { get; set; }
        public Guid UserId { get; set; }
        public bool IsUsed { get; set; }
    }
}
