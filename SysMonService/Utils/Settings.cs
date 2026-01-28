namespace SysMonService.Utils;

public class Settings(IConfiguration configuration)
{
    public string? ComPort => configuration.GetValue<string>("Settings:ComPort");
    
    public double? PollingRate => configuration.GetValue<double?>("Settings:PollingRate");
    
    public bool? RetryOpen => configuration.GetValue<bool?>("Settings:RetryOpen");
    
    public int? RetryAttempts => configuration.GetValue<int?>("Settings:RetryAttempts");
}