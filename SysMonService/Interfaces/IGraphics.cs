namespace SysMonService.Interfaces;

/// <summary>
/// Represents a Graphics Processing Unit (GPU) and its associated telemetry data.
/// Inherits base hardware properties from <see cref="IHardware"/>
/// </summary>
public interface IGraphics : IHardware
{
    /// <summary>
    /// Gets or sets the current GPU utilisation as a percentage (0 to 100).
    /// </summary>
    decimal? UsagePercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the current GPU core clock frequency in MHz.
    /// </summary>
    decimal? ClockSpeed { get; set; }
    
    /// <summary>
    /// Gets or sets the current GPU temperature in degrees Celsius.
    /// </summary>
    decimal? Temperature { get; set; }
}