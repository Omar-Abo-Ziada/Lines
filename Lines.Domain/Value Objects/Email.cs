using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lines.Domain.Value_Objects
{
    
    public record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            // Allow empty for EF Core's parameterless constructor materialization
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = string.Empty;
                return;
            }
            
            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email");

            Value = FormatEmail(value);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Basic email validation regex
                var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, emailRegex);
            }
            catch
            {
                return false;
            }
        }

        private string FormatEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return email;

            // Convert to lowercase and trim whitespace
            return email.Trim().ToLowerInvariant();
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }

}
