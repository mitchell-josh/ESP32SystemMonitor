namespace SysMonService.Interfaces;

public interface ISettings
{
    public string? ComPort { get; }
    
    public double? PollingRate { get; }
}