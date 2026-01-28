namespace SysMonService.Interfaces;

public interface INetwork : IHardware
{
    decimal? Sent { get; set; }
    
    decimal? Received { get; set; }
}