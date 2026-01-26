using Hardware.Info;
using SysMonService.Interfaces;

namespace SysMonService.Models;

public class Processor(IHardwareProvider hardwareProvider) : SimpleSerialiser<Processor>, IProcessor
{
    public double? UsagePercentage { get; set; }
    
    public double? ClockSpeed { get; set; }
    
    public double? Temperature { get; set; }

    public void Refresh()
    {
        this.UsagePercentage = hardwareProvider.GetCpuUsage();
        this.ClockSpeed = hardwareProvider.GetCpuClockSpeed();
        this.Temperature = hardwareProvider.GetCpuTemperature();
    }
}