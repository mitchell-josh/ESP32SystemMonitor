namespace SysMonService.Interfaces;

public interface IMemory : IHardware
{
    double? UsedMemory { get; set; }
    
    double? TotalMemory { get; set; }
}