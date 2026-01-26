namespace SysMonService.Interfaces;

public interface IProcessor : IHardware
{
    double? UsagePercentage { get; set; }

    double? ClockSpeed { get; set; }
    
    double? Temperature { get; set; }
}