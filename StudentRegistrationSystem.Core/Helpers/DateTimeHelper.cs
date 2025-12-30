using System;

namespace StudentRegistrationSystem.Core.Helpers;

public static class DateTimeHelper
{
    public static DateTime UtcNow => DateTime.UtcNow;

    public static DateTime UtcToday => DateTime.UtcNow.Date;

   
    public static DateTime AddHours(int hours)
    {
        return DateTime.UtcNow.AddHours(hours);
    }

    public static DateTime AddDays(int days)
    {   
        return DateTime.UtcNow.AddDays(days);
    }

    public static bool IsPast(DateTime? date)
    {
        if (!date.HasValue)
            return false;
        
        return date.Value < DateTime.UtcNow;
    }

    public static bool IsFuture(DateTime? date)
    {
        if (!date.HasValue)
            return false;
        
        return date.Value > DateTime.UtcNow;
    }

    public static bool IsTodayOrPast(DateTime? date)
    {
        if (!date.HasValue)
            return false;
        
        return date.Value.Date <= DateTime.UtcNow.Date;
    }
}
