using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.User
{
    public class Otp : BaseModel
    {
        public string Code { get; set; }
        public DateTime OTPGenerationTime { get; set; }
        public Guid UserId { get; set; }
        public bool IsUsed { get; private set; } 
        private static readonly int OtpLength = 6;
        private static readonly int ExpiryMinutes = 10;

        public Otp(string code, Guid userId, DateTime? otpGenerationTime = null) // pass generation time when u are creating obj that represents already existing otp in db , do not pass if u are creating new otp to be saved later in db
        {
            if (!IsValidFormat(code))
                throw new ArgumentException($"OTP code must be {OtpLength} digits.");
            Code = code;
            UserId = userId;
            IsUsed = false;
            OTPGenerationTime = otpGenerationTime ?? DateTime.Now;
        }


        // for data seeding
        public Otp()
        {

        }

        public bool IsExpired()
        {
            return DateTime.Now > OTPGenerationTime.AddMinutes(ExpiryMinutes);
        }

        public bool IsValid(string code)  
        {
            return !IsUsed && !IsExpired() && Code == code;
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        private static bool IsValidFormat(string code)
        {
            return !string.IsNullOrEmpty(code) && code.Length == OtpLength && code.All(char.IsDigit);
        }
    }
}
