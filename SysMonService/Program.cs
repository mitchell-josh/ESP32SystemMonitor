using SysMonService;
using SysMonService.HardwareProviders;
using SysMonService.Interfaces;
using SysMonService.Utils;
using ISettings = SysMonService.Interfaces.ISettings;

var builder = Host.CreateApplicationBuilder(args);

// Register the hardware provider as a singleton.
// Ensures only a single instance of LibreHardwareMonitor is running.
builder.Services.AddSingleton<IHardwareProvider, WindowsHardwareProvider>();

// Register settings to be accessible throughout the application.
builder.Services.AddSingleton<ISettings, Settings>();

// Secondary worker service for general tasks (i.e. Logging)
builder.Services.AddHostedService<Worker>();

// Registers the Machine model.
// Manually injects the required IHardwareProvider and ISetting into the machine.
builder.Services.AddSingleton<SysMonService.Models.Machine>(sp 
    => new SysMonService.Models.Machine(
        sp.GetRequiredService<IHardwareProvider>(), 
        sp.GetRequiredService<ISettings>()));

// Register the core SystemMonitorService that handles refreshing data
// and sending it over the COM port.
builder.Services.AddHostedService<SystemMonitorService>();

var host = builder.Build();

await host.RunAsync();

