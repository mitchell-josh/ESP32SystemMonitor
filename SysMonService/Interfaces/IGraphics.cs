namespace SysMonService.Interfaces;

public interface IGraphics : IHardware
{
    decimal? UsagePercentage { get; set; }
    
    decimal? ClockSpeed { get; set; }
    
    decimal? Temperature { get; set; }
}