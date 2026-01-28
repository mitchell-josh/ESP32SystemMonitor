namespace SysMonService.Utils;

/// <summary>
/// Provides extension methods to handle hardware-specific unit conversions and
/// rounding logic.
/// </summary>
public static class Converters
{
    /// <summary>
    /// Converts a frequency value from Megahertz to Gigahertz.
    /// </summary>
    /// <param name="mhz">The frequency in MHz.</param>
    /// <returns>The frequency in GHz, or 0.0 if input is null.</returns>
    public static decimal? MhzToGhz(this decimal? mhz)
    {
        return (mhz ?? 0.0m) / 1000.0m;
    }
    
    /// <summary>
    /// Converts data from raw Bytes to Megabits per second (Mbps).
    /// </summary>
    /// <param name="bytes">The data rate in bytes.</param>
    /// <returns>The data rate in Mbps, calculated as (Bytes * 8) / 1,000,000.</returns>
    public static decimal? BytesToMbps(this decimal? bytes)
    {
        return ((bytes ?? 0m) * 8.0m) / 1000000.0m;
    }

    /// <summary>
    /// Rounds a nullable decimal to 2 decimal places using standard mathematical rounding.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <returns>The rounded value, or 0.0 if null.</returns>
    public static decimal? Round2(this decimal? value)
    {
        return value.Round(2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Internal helper to perform rounding with a specified precision and mode.
    /// Defaults to 'AwayFromZero' to ensure .5 values round up.
    /// </summary>
    private static decimal? Round(
        this decimal? value, 
        int roundingPlaces, 
        MidpointRounding roundingMode =  MidpointRounding.AwayFromZero)
    {
        return Math.Round((value ?? 0.0m), roundingPlaces, roundingMode);
    }
}