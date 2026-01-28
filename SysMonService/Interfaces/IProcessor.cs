namespace SysMonService.Interfaces;

/// <summary>
/// Represents the Central Processing Unit (CPU) and its primary performance metrics.
/// Inherits base update capabilities from <see cref="IHardware"/>.
/// </summary>
public interface IProcessor : IHardware
{
    /// <summary>
    /// Gets or sets the average CPU load across all cores as a percentage (0 to 100).
    /// </summary>
    decimal? UsagePercentage { get; set; }

    /// <summary>
    /// Gets or sets the maximum active operating frequency of the processor in GHz.
    /// </summary>
    decimal? ClockSpeed { get; set; }
    
    /// <summary>
    /// Gets or sets the thermal state of the CPU package in degrees Celsius.
    /// </summary>
    decimal? Temperature { get; set; }
}