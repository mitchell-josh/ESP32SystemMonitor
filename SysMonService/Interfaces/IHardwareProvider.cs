namespace SysMonService.Interfaces;

public interface IHardwareProvider
{
    decimal? GetCpuUsage();
    
    decimal? GetCpuClockSpeed();

    decimal? GetCpuTemperature();

    decimal? GetUsedMemory();
    
    decimal? GetGpuUsage();
    
    decimal? GetGpuClockSpeed();

    decimal? GetGpuTemperature();

    decimal? GetNetworkSent();
    
    decimal? GetNetworkReceived();
}