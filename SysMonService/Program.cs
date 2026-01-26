using LibreHardwareMonitor.Hardware;
using SysMonService;
using SysMonService.HardwareProviders;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

SysMonService.Models.System system = new(new WindowsHardwareProvider(new Computer()));

#if DEBUG
System.Diagnostics.Debug.WriteLine(system.Serialise());
#endif