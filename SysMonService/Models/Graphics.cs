using System.Text.Json;
using System.Text.Json.Serialization;
using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

public class Graphics(IHardwareProvider hardwareProvider) : SimpleSerialiser<Graphics>, IGraphics
{
    public decimal? UsagePercentage { get; set; }
    
    public decimal? ClockSpeed { get; set; }

    public decimal? Temperature { get; set; }
    
    public void Refresh()
    {
        this.UsagePercentage = hardwareProvider.GetGpuUsage().Round2();
        this.ClockSpeed = hardwareProvider.GetGpuClockSpeed().Round2();
        this.Temperature =  hardwareProvider.GetGpuTemperature().Round2();
    }
}