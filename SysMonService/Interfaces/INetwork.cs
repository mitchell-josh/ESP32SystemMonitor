namespace SysMonService.Interfaces;

public interface INetwork : IHardware
{
    double? Sent { get; set; }
    
    double? Received { get; set; }
}