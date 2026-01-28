using System.Configuration.Internal;
using LibreHardwareMonitor.Hardware;
using SysMonService.Interfaces;
using SysMonService.Models;

namespace SysMonService.HardwareProviders;

public class WindowsHardwareProvider : IHardwareProvider
{
    private readonly Computer _computer;

    private readonly WindowsHardwareVisitor _visitor;

    public WindowsHardwareProvider(Computer? computer = null)
    {
        this._computer = computer ?? new Computer
        {
            IsCpuEnabled = true,
            IsMemoryEnabled = true,
            IsGpuEnabled = true,
            IsNetworkEnabled = true,
        };

        this._computer.Open();

        this._visitor = new WindowsHardwareVisitor();
    }

    private void Refresh()
    {
        this._computer.Accept(this._visitor);
    }

    public decimal? GetCpuUsage()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total")
            ?.Value;
    }

    public decimal? GetCpuClockSpeed()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.Where(s => s.SensorType == SensorType.Clock && s.Name.Contains("Cores (Average)"))
            ?.Max(s => s.Value);
    }

    public decimal? GetCpuTemperature()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "Core (Tctl/Tdie)")
            ?.Value;
    }

    public decimal? GetUsedMemory()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Memory && h.Name == "Total Memory")
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Used")
            ?.Value;
    }

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

    public decimal? GetNetworkSent()
    {
        this.Refresh();
        return (decimal?)this._computer.Hardware
            ?.Where(h => h.HardwareType == HardwareType.Network)
            ?.SelectMany(h => h.Sensors)
            ?.Where(s => s.SensorType == SensorType.Throughput && s.Name == "Upload Speed")
            ?.Max(s => s.Value);
    }

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