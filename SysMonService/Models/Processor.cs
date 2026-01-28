using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

public class Processor(IHardwareProvider hardwareProvider) : SimpleSerialiser<Processor>, IProcessor
{
    public decimal? UsagePercentage { get; set; }
    
    public decimal? ClockSpeed { get; set; }
    
    public decimal? Temperature { get; set; }

    public void Refresh()
    {
        this.UsagePercentage = hardwareProvider.GetCpuUsage().Round2();
        this.ClockSpeed = hardwareProvider.GetCpuClockSpeed().MhzToGhz().Round2();
        this.Temperature = hardwareProvider.GetCpuTemperature().Round2();
    }
}