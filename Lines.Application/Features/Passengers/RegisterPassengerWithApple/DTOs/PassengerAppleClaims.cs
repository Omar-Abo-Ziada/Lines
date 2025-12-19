using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs
{ 
    public record PassengerAppleClaims(string FirstName, string LastName, string Email, string AppleProviderKey);
}
