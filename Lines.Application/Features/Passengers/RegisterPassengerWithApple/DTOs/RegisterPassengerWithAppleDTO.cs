using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs
{
    public class RegisterPassengerWithAppleDTO
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
