namespace SysMonService.Interfaces;

public interface IHardwareProvider
{
    double? GetCpuUsage();
    
    double? GetCpuClockSpeed();

    double? GetCpuTemperature();

    double? GetUsedMemory();

    double? GetTotalMemory();

    double? GetGpuUsage();
    
    double? GetGpuClockSpeed();

    double? GetGpuTemperature();

    double? GetNetworkSent();
    
    double? GetNetworkReceived();
}