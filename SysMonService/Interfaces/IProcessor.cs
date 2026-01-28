namespace SysMonService.Interfaces;

public interface IProcessor : IHardware
{
    decimal? UsagePercentage { get; set; }

    decimal? ClockSpeed { get; set; }
    
    decimal? Temperature { get; set; }
}