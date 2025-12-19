using AdminLine.Common.DTOs;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;

namespace AdminLine.Service.Helpers;

public static class DriverStatusHelper
{
    public static DriverStatus DetermineDriverStatus(Driver driver)
    {
        if (driver.IsDeleted)
        {
            // Deleted drivers shouldn't appear in the list, but handle edge case
            return DriverStatus.Suspended;
        }

        // PendingReview: Registration status is PendingReview
        if (driver.RegistrationStatus == RegistrationStatus.PendingReview)
        {
            return DriverStatus.PendingReview;
        }

        // ProfileUpdate: Verified but registration is incomplete
        if (driver.RegistrationStatus == RegistrationStatus.Verified && !driver.IsRegistrationComplete())
        {
            return DriverStatus.ProfileUpdate;
        }

        // Active: Verified, complete registration, and available
        if (driver.RegistrationStatus == RegistrationStatus.Verified && driver.IsAvailable)
        {
            return DriverStatus.Active;
        }

        // Suspended: Verified but not available (or other suspension conditions)
        if (driver.RegistrationStatus == RegistrationStatus.Verified && !driver.IsAvailable)
        {
            return DriverStatus.Suspended;
        }

        // Default to PendingReview for other cases
        return DriverStatus.PendingReview;
    }

    public static string FormatLastActivity(DateTime? lastActivity)
    {
        if (!lastActivity.HasValue)
        {
            return "Never";
        }

        var timeSpan = DateTime.UtcNow - lastActivity.Value;

        if (timeSpan.TotalMinutes < 1)
        {
            return "Just now";
        }
        else if (timeSpan.TotalMinutes < 60)
        {
            var minutes = (int)timeSpan.TotalMinutes;
            return $"{minutes} {(minutes == 1 ? "minute" : "minutes")} ago";
        }
        else if (timeSpan.TotalHours < 24)
        {
            var hours = (int)timeSpan.TotalHours;
            return $"{hours} {(hours == 1 ? "hour" : "hours")} ago";
        }
        else if (timeSpan.TotalDays < 30)
        {
            var days = (int)timeSpan.TotalDays;
            return $"{days} {(days == 1 ? "day" : "days")} ago";
        }
        else if (timeSpan.TotalDays < 365)
        {
            var months = (int)(timeSpan.TotalDays / 30);
            return $"{months} {(months == 1 ? "month" : "months")} ago";
        }
        else
        {
            var years = (int)(timeSpan.TotalDays / 365);
            return $"{years} {(years == 1 ? "year" : "years")} ago";
        }
    }

    public static string GenerateDriverCode(Guid driverId)
    {
        // Generate driver code like P001, P002, etc.
        // Using first 8 characters of GUID converted to a number, then format as P{number}
        var guidBytes = driverId.ToByteArray();
        var number = Math.Abs(BitConverter.ToInt32(guidBytes, 0)) % 999999;
        return $"P{number:D6}";
    }
}

