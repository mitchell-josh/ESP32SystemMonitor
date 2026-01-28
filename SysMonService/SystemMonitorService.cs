using System.IO.Ports;
using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService;

public class SystemMonitorService(Models.System system) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var serialPort = new SerialPort(system.Settings.ComPort, 9600, Parity.None, 8, StopBits.One);
        
        serialPort.DataReceived += (sender, e) =>
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            global::System.Diagnostics.Debug.WriteLine($"[ESP32 DEBUG]: {indata}");
        };

        serialPort.DtrEnable = true;
        serialPort.RtsEnable = true;
        serialPort.Open();
        
        await AsyncUtils.StartAsync(() =>
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(system.RefreshAndSerialise().Trim() + "\r\n");
            serialPort.BaseStream.Write(data, 0, data.Length);
        }, stoppingToken, 2000);
    }
}