using Lines.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly int OtpLength = 6;
        public string GenerateRandomOtp()
        {
            const string chars = "0123456789"; // Characters to use for OTP generation
            var random = new Random();
            return new string(Enumerable.Repeat(chars, OtpLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
