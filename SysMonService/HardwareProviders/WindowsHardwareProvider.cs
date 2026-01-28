using System.Configuration.Internal;
using LibreHardwareMonitor.Hardware;
using SysMonService.Interfaces;
using SysMonService.Models;

namespace SysMonService.HardwareProviders;

/// <summary>
/// Interface for retrieving real-time telemetry for Windows machines.
/// </summary>
public class WindowsHardwareProvider : IHardwareProvider
{
    /// <summary>
    /// LibreHardwareMonitor object used to interface with system drivers.
    /// </summary>
    private readonly Computer _computer;

    /// <summary>
    /// Visitor pattern implementation used to traverse and update hardware components.
    /// </summary>
    private readonly WindowsHardwareVisitor _visitor;

    /// <summary>
    /// Initialises a new instance of the <see cref="WindowsHardwareProvider"/>
    /// Configures and opens the hardware monitoring session.
    /// </summary>
    /// <param name="computer">
    /// Optional, pre-configured <see cref="Computer"/> instance.
    /// If null, a new instance is created with CPU, RAM, GPU and Network monitoring. enabled.</param>
    /// <remarks>
    /// NOTE: The constructor calls _computer.Open() which requires administrator privileges to access
    /// certain hardware registers.
    /// </remarks>
    public WindowsHardwareProvider(Computer? computer = null)
    {
        // Use provided computer or initialize a default one with required sensors active
        this._computer = computer ?? new Computer
        {
            IsCpuEnabled = true,
            IsMemoryEnabled = true,
            IsGpuEnabled = true,
            IsNetworkEnabled = true,
        };

        // Establishes the connection to the hardware drivers
        this._computer.Open();

        // Visitor handles the logic for navigating the hardware tree
        this._visitor = new WindowsHardwareVisitor();
    }

    /// <summary>
    /// Refreshes the telemetry data.
    /// </summary>
    private void Refresh()
    {
        this._computer.Accept(this._visitor);
    }

    /// <summary>
    /// Gets the current total CPU utilisation as a percentage (0-100).
    /// </summary>
    public decimal? GetCpuUsage()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total")
            ?.Value;
    }

    /// <summary>
    /// Gets the current maximum clock speed across all CPU cores in GHz.
    /// </summary>
    public decimal? GetCpuClockSpeed()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.Where(s => s.SensorType == SensorType.Clock && s.Name.Contains("Cores (Average)"))
            ?.Max(s => s.Value);
    }

    /// <summary>
    /// Gets the current package temperature of the CPU in degrees Celsius.
    /// </summary>
    public decimal? GetCpuTemperature()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "Core (Tctl/Tdie)")
            ?.Value;
    }

    /// <summary>
    /// Gets the total amount of physical memory currently in use (GB).
    /// </summary>
    public decimal? GetUsedMemory()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Memory && h.Name == "Total Memory")
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Used")
            ?.Value;
    }

    /// <summary>
    /// Gets the current total GPU utilisation as a percentage (0-100). 
    /// </summary>
    public decimal? GetGpuUsage()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => (h.HardwareType == HardwareType.GpuAmd
                                        || h.HardwareType == HardwareType.GpuNvidia
                                        || h.HardwareType == HardwareType.GpuIntel))
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "GPU Core")
            ?.Value;        
    }
    
    /// <summary>
    /// Gets the current GPU core clock frequency in MHz.
    /// </summary>
    public decimal? GetGpuClockSpeed()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => (h.HardwareType == HardwareType.GpuAmd
                                   || h.HardwareType == HardwareType.GpuNvidia
                                   || h.HardwareType == HardwareType.GpuIntel))
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Clock && s.Name == "GPU Core")
            ?.Value;   
    }
    
    /// <summary>
    /// Gets the current temperature of the GPU core in degrees Celsius.
    /// </summary>
    public decimal? GetGpuTemperature()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => (h.HardwareType == HardwareType.GpuAmd
                                   || h.HardwareType == HardwareType.GpuNvidia
                                   || h.HardwareType == HardwareType.GpuIntel))
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "GPU Core")
            ?.Value;       
    }

    /// <summary>
    /// Gets the current outgoing network throughput in megabytes per second.
    /// </summary>
    public decimal? GetNetworkSent()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.Where(h => h.HardwareType == HardwareType.Network)
            ?.SelectMany(h => h.Sensors)
            ?.Where(s => s.SensorType == SensorType.Throughput && s.Name == "Upload Speed")
            ?.Max(s => s.Value);
    }

    /// <summary>
    /// Gets the current incoming network throughput in megabytes per second.
    /// </summary>
    public decimal? GetNetworkReceived()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.Where(h => h.HardwareType == HardwareType.Network)
            ?.SelectMany(h => h.Sensors)
            ?.Where(s => s.SensorType == SensorType.Throughput && s.Name == "Download Speed")
            ?.Max(s => s.Value);     
    }
}