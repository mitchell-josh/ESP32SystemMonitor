using Hardware.Info;
using LibreHardwareMonitor.Hardware;

namespace SysMonService.Models;

public class Hardware
{
    public Hardware(HardwareInfo? hardwareInfo, Computer? computer)
    {
        this.IsWindows = OperatingSystem.IsWindows();
        this.IsLinux = OperatingSystem.IsLinux();
        
        this.HardwareInfo = HardwareInfo ?? new HardwareInfo();

        this.Computer = Computer ?? new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsNetworkEnabled = true,
        };
    }
    
    public IHardwareInfo? HardwareInfo { get; set; }

    public Computer? Computer { get; set; }

    public bool IsWindows { get; }
    
    public bool IsLinux { get; }
}