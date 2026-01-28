using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

/// <summary>
/// Represents a Graphics Processing Unit (GPU) and its associated telemetry data.
/// Inherits base hardware properties from <see cref="IHardware"/>
/// </summary>
public class Graphics(IHardwareProvider hardwareProvider) : SimpleSerialiser<Graphics>, IGraphics
{
    /// <summary>
    /// Gets or sets the current GPU utilisation as a percentage (0 to 100).
    /// </summary>
    public decimal? UsagePercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the current GPU core clock frequency in MHz.
    /// </summary>
    public decimal? ClockSpeed { get; set; }

    /// <summary>
    /// Gets or sets the current GPU temperature in degrees Celsius.
    /// </summary>
    public decimal? Temperature { get; set; }
    
    /// <summary>
    /// Triggers a fresh poll of the hardware sensors.
    /// This method should be called immediately before accessing telemetry properties to ensure the data is up-to-date
    /// and not cached.
    /// </summary>
    public void Refresh()
    {
        this.UsagePercentage = hardwareProvider.GetGpuUsage().Round2();
        this.ClockSpeed = hardwareProvider.GetGpuClockSpeed().Round2();
        this.Temperature =  hardwareProvider.GetGpuTemperature().Round2();
    }
}