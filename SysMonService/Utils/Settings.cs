namespace SysMonService.Utils;

public class Settings(IConfiguration configuration)
{
    public string? ComPort => configuration.GetValue<string>("Settings:ComPort");
    
    public double? PollingRate => configuration.GetValue<double?>("Settings:PollingRate");
}