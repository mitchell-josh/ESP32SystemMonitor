using System.IO.Ports;
using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService;

public class SystemMonitorService(Models.System system) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SerialPort serialPort = null!;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!(serialPort?.IsOpen ?? false))
                {
                    serialPort?.Dispose();

                    serialPort = SerialUtils.GetOpenSerialPort(
                        system.Settings.ComPort!,
                        9600,
                        Parity.None,
                        8,
                        StopBits.One);
                }

                if (serialPort?.IsOpen ?? false)
                {
                    var data = System.Text.Encoding.ASCII.GetBytes(system.RefreshAndSerialise().Trim() + "\r\n");
                    await serialPort.BaseStream.WriteAsync(data, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                serialPort?.Close();
            }
            
            await Task.Delay((int)system.Settings.PollingRate!, stoppingToken);
        }
    }
}