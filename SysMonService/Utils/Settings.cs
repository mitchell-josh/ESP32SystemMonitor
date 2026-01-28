using SysMonService.Interfaces;

namespace SysMonService.Utils;

/// <summary>
/// Defines the configuration parameters for the system monitor service.
/// </summary>
public class Settings(IConfiguration configuration) : ISettings
{
    /// <summary>
    /// Gets the identifier of the serial communication port (i.e. COM3)
    /// </summary>
    public string? ComPort => configuration.GetValue<string>("Settings:ComPort");
    
    /// <summary>
    /// Gets the interval in milliseconds at which data is transmitted.
    /// </summary>
    public int? PollingRate => configuration.GetValue<int?>("Settings:PollingRate");
}