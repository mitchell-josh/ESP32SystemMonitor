using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

public class Memory(IHardwareProvider hardwareProvider) : SimpleSerialiser<Memory>, IMemory
{
    public decimal? UsedMemory { get; set; }
    
    public void Refresh()
    {
        this.UsedMemory = hardwareProvider.GetUsedMemory().Round2();
    }
}