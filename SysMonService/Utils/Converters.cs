namespace SysMonService.Utils;

public static class Converters
{
    public static decimal? MhzToGhz(this decimal? mhz)
    {
        return (mhz ?? 0.0m) / 1000.0m;
    }
    
    public static decimal? BytesToMbps(this decimal? bytes)
    {
        return ((bytes ?? 0m) * 8.0m) / 1000000.0m;
    }

    public static decimal? Round2(this decimal? value)
    {
        return value.Round(2, MidpointRounding.AwayFromZero);
    }

    private static decimal? Round(
        this decimal? value, 
        int roundingPlaces, 
        MidpointRounding roundingMode =  MidpointRounding.AwayFromZero)
    {
        return Math.Round((value ?? 0.0m), roundingPlaces, roundingMode);
    }
}