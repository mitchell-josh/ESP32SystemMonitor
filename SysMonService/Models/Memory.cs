using SysMonService.Interfaces;

namespace SysMonService.Models;

public class Memory(IHardwareProvider hardwareProvider) : SimpleSerialiser<Memory>, IMemory
{
    public double? UsedMemory { get; set; }
    
    public double? TotalMemory { get; set; }

    public void Refresh()
    {
        this.UsedMemory = hardwareProvider.GetUsedMemory();
        this.TotalMemory = hardwareProvider.GetTotalMemory();
    }
}