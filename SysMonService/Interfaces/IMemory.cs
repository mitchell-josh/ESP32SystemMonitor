namespace SysMonService.Interfaces;

public interface IMemory : IHardware
{
    decimal? UsedMemory { get; set; }
}