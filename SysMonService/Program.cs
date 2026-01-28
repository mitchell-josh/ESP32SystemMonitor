using SysMonService;
using SysMonService.HardwareProviders;
using SysMonService.Interfaces;
using SysMonService.Utils;
using ISettings = SysMonService.Interfaces.ISettings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IHardwareProvider, WindowsHardwareProvider>();

builder.Services.AddSingleton<ISettings, Settings>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<SysMonService.Models.System>(sp 
    => new SysMonService.Models.System(
        sp.GetRequiredService<IHardwareProvider>(), 
        sp.GetRequiredService<ISettings>()));

builder.Services.AddHostedService<SystemMonitorService>();

var host = builder.Build();

await host.RunAsync();

