using Lines.Domain.Models.User;

namespace Lines.Application.Features.Otps.ValidateOtpByUserId.DTOs
{
    public class ValidateOtpByUserIdDto
    {
        public bool IsValid { get; set; } 
        public Otp? Otp { get; set; }  // if not valid do not return the otp retrieved from db

        // Enable deconstruction
        public void Deconstruct(out bool isValid, out Otp? otp)
        {
            isValid = IsValid;
            otp = Otp;
        }
    }
}
