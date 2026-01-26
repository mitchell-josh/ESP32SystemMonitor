using System.Configuration.Internal;
using LibreHardwareMonitor.Hardware;
using SysMonService.Interfaces;
using SysMonService.Models;

namespace SysMonService.HardwareProviders;

public class WindowsHardwareProvider : IHardwareProvider
{
    private readonly Computer _computer;

    private readonly WindowsHardwareVisitor _visitor;

    public WindowsHardwareProvider(Computer? computer)
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

    public double? GetCpuUsage()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total")
            ?.Value;
    }

    public double? GetCpuClockSpeed()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.Where(s => s.SensorType == SensorType.Clock)
            ?.Average(s => s.Value);
    }

    public double? GetCpuTemperature()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "CPU Package")
            ?.Value;
    }

    public double? GetUsedMemory()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Memory)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Used")
            ?.Value;
    }

    public double? GetTotalMemory()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => h.HardwareType == HardwareType.Memory)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Available")
            ?.Value;    
    }

    public double? GetGpuUsage()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => (h.HardwareType == HardwareType.GpuAmd
                                        || h.HardwareType == HardwareType.GpuNvidia
                                        || h.HardwareType == HardwareType.GpuIntel))
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "GPU Core")
            ?.Value;        
    }

    public double? GetGpuTemperature()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => (h.HardwareType == HardwareType.GpuAmd
                                   || h.HardwareType == HardwareType.GpuNvidia
                                   || h.HardwareType == HardwareType.GpuIntel))
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "GPU Core")
            ?.Value;       
    }

    public double? GetGpuClockSpeed()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h => (h.HardwareType == HardwareType.GpuAmd
                                   || h.HardwareType == HardwareType.GpuNvidia
                                   || h.HardwareType == HardwareType.GpuIntel))
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Clock && s.Name == "GPU Core")
            ?.Value;   
    }

    public double? GetNetworkSent()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h =>  h.HardwareType == HardwareType.Network)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Throughput && s.Name.Contains("Upload"))
            ?.Value;       
    }

    public double? GetNetworkReceived()
    {
        this.Refresh();
        return this._computer.Hardware
            ?.FirstOrDefault(h =>  h.HardwareType == HardwareType.Network)
            ?.Sensors
            ?.FirstOrDefault(s => s.SensorType == SensorType.Throughput && s.Name.Contains("Download"))
            ?.Value;      
    }
}