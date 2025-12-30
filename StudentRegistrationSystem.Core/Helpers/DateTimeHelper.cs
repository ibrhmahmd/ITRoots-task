using System;

namespace StudentRegistrationSystem.Core.Helpers;

/// <summary>
/// Helper class for date and time operations
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Gets the current UTC date and time
    /// </summary>
    public static DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Gets the current date (UTC)
    /// </summary>
    public static DateTime UtcToday => DateTime.UtcNow.Date;

    /// <summary>
    /// Adds hours to the current UTC time
    /// </summary>
    public static DateTime AddHours(int hours)
    {
        return DateTime.UtcNow.AddHours(hours);
    }

    /// <summary>
    /// Adds days to the current UTC time
    /// </summary>
    public static DateTime AddDays(int days)
    {
        return DateTime.UtcNow.AddDays(days);
    }

    /// <summary>
    /// Checks if a date is in the past
    /// </summary>
    public static bool IsPast(DateTime? date)
    {
        if (!date.HasValue)
            return false;
        
        return date.Value < DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if a date is in the future
    /// </summary>
    public static bool IsFuture(DateTime? date)
    {
        if (!date.HasValue)
            return false;
        
        return date.Value > DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if a date is today or in the past
    /// </summary>
    public static bool IsTodayOrPast(DateTime? date)
    {
        if (!date.HasValue)
            return false;
        
        return date.Value.Date <= DateTime.UtcNow.Date;
    }
}
