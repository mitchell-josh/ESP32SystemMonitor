using System.Text.Json;
using System.Text.Json.Serialization;
using SysMonService.Interfaces;

namespace SysMonService.Models;

public class Graphics(IHardwareProvider hardwareProvider) : SimpleSerialiser<Graphics>, IGraphics
{
    public double? UsagePercentage { get; set; }
    
    public double? ClockSpeed { get; set; }

    public double? Temperature { get; set; }
    
    public void Refresh()
    {
        this.UsagePercentage = hardwareProvider.GetGpuUsage();
        this.ClockSpeed = hardwareProvider.GetGpuClockSpeed();
        this.Temperature =  hardwareProvider.GetGpuTemperature();
    }
}