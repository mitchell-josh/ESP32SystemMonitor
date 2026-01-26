namespace SysMonService.Interfaces;

public interface IGraphics : IHardware
{
    double? UsagePercentage { get; set; }
    
    double? ClockSpeed { get; set; }
    
    double? Temperature { get; set; }
}