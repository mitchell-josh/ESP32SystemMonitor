using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

/// <summary>
/// Represents the Central Processing Unit (CPU) and its primary performance metrics.
/// Inherits base update capabilities from <see cref="IHardware"/>.
/// </summary>s
public class Processor(IHardwareProvider hardwareProvider) : SimpleSerialiser<Processor>, IProcessor
{
    /// <summary>
    /// Gets or sets the average CPU load across all cores as a percentage (0 to 100).
    /// </summary>
    public decimal? UsagePercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum active operating frequency of the processor in GHz.
    /// </summary>
    public decimal? ClockSpeed { get; set; }
    
    /// <summary>
    /// Gets or sets the thermal state of the CPU package in degrees Celsius.
    /// </summary>
    public decimal? Temperature { get; set; }

    /// <summary>
    /// Triggers a fresh poll of CPU hardware sensors.
    /// This method should be called immediately before accessing telemetry properties to ensure the data is up-to-date
    /// and not cached.
    /// </summary>
    public void Refresh()
    {
        this.UsagePercentage = hardwareProvider.GetCpuUsage().Round2();
        this.ClockSpeed = hardwareProvider.GetCpuClockSpeed().MhzToGhz().Round2();
        this.Temperature = hardwareProvider.GetCpuTemperature().Round2();
    }
}