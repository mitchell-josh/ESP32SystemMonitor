using System.IO.Ports;
using LibreHardwareMonitor.Hardware;
using SysMonService;
using SysMonService.HardwareProviders;
using SysMonService.Interfaces;
using SysMonService.Utils;

var builder = Host.CreateApplicationBuilder(args);

// Run this and look at your Debug/Console window
string[] ports = SerialPort.GetPortNames();
Console.WriteLine("--- AVAILABLE PORTS ---");
foreach (string p in ports) Console.WriteLine(p);

builder.Services.AddSingleton<IHardwareProvider, WindowsHardwareProvider>();

builder.Services.AddSingleton<Settings>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<SysMonService.Models.System>(sp 
    => new SysMonService.Models.System(
        sp.GetRequiredService<IHardwareProvider>(), 
        sp.GetRequiredService<Settings>()));

builder.Services.AddHostedService<SystemMonitorService>();

var host = builder.Build();

await host.RunAsync();

