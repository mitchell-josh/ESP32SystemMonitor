namespace SysMonService.Interfaces;

/// <summary>
/// Interface for retrieving real-time telemetry from system hardware.
/// </summary>
public interface IHardwareProvider
{
    /// <summary>
    /// Gets the current total CPU utilisation as a percentage (0-100).
    /// </summary>
    decimal? GetCpuUsage();
    
    /// <summary>
    /// Gets the current maximum clock speed across all CPU cores in GHz.
    /// </summary>
    decimal? GetCpuClockSpeed();

    /// <summary>
    /// Gets the current package temperature of the CPU in degrees Celsius.
    /// </summary>
    decimal? GetCpuTemperature();

    /// <summary>
    /// Gets the total amount of physical memory currently in use (GB).
    /// </summary>
    decimal? GetUsedMemory();
    
    /// <summary>
    /// Gets the current total GPU utilisation as a percentage (0-100). 
    /// </summary>
    decimal? GetGpuUsage();
    
    /// <summary>
    /// Gets the current GPU core clock frequency in MHz.
    /// </summary>
    decimal? GetGpuClockSpeed();

    /// <summary>
    /// Gets the current temperature of the GPU core in degrees Celsius.
    /// </summary>
    decimal? GetGpuTemperature();

    /// <summary>
    /// Gets the current outgoing network throughput in megabytes per second.
    /// </summary>
    decimal? GetNetworkSent();
    
    /// <summary>
    /// Gets the current incoming network throughput in megabytes per second.
    /// </summary>
    decimal? GetNetworkReceived();
}