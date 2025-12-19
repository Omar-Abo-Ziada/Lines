using System.Text.RegularExpressions;

namespace Lines.Domain.Value_Objects;


public record PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        // Allow empty for EF Core's parameterless constructor materialization
        if (string.IsNullOrWhiteSpace(value))
        {
            Value = string.Empty;
            return;
        }

        //if (!IsValidPhoneNumber(value))
        //    throw new ArgumentException("Invalid phone number");

        Value = FormatPhoneNumber(value);
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Remove all non-digit characters except + for validation
        var cleanNumber = Regex.Replace(phoneNumber, @"[^\d+]", "");

        // Check if it starts with + (international format)
        if (cleanNumber.StartsWith("+"))
        {
            // International format: + followed by 7-15 digits
            return cleanNumber.Length >= 8 && cleanNumber.Length <= 16 &&
                   Regex.IsMatch(cleanNumber, @"^\+\d{7,15}$");
        }
        else
        {
            // National format: 7-15 digits
            return cleanNumber.Length >= 7 && cleanNumber.Length <= 15 &&
                   Regex.IsMatch(cleanNumber, @"^\d{7,15}$");
        }
    }

    private string FormatPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return phoneNumber;

        // Remove all non-digit characters except +
        var cleanNumber = Regex.Replace(phoneNumber, @"[^\d+]", "");

        // If it doesn't start with +, assume it's a national number
        if (!cleanNumber.StartsWith("+"))
        {
            // Add country code if missing (assuming Saudi Arabia +966)
            // You can modify this based on your business requirements
            if (cleanNumber.Length >= 7 && cleanNumber.Length <= 15)
            {
                return "+966" + cleanNumber;
            }
        }

        return cleanNumber;
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}
