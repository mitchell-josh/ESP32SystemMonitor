using System.IO.Ports;
using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService;

/// <summary>
/// Primary background service responsible for orchestrating hardware data connection
/// and maintaining the serial connection to the microcontroller.
/// </summary>
/// <param name="machine">Model representing machine hardware state.</param>
public class SystemMonitorService(Models.Machine machine) : BackgroundService
{
    /// <summary>
    /// Main execution loop of service.
    /// Handles automatic reconnection and periodic data transmission.
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SerialPort serialPort = null!;
        
        // Loop runs for entire lifetime of the service.
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // If port is null or has been closed/disconnected, attempt to reconnect.
                if (!(serialPort?.IsOpen ?? false))
                {
                    serialPort?.Dispose();
                    serialPort = null; // tidy up object

                    // Blocking call that waits for hardware to become available.
                    serialPort = SerialUtils.GetOpenSerialPort(
                        machine.Settings.ComPort!,
                        9600,
                        Parity.None,
                        8,
                        StopBits.One);
                }
                
                // Attempt to serialise and send data to microcontroller if connection is open.
                if (serialPort?.IsOpen ?? false)
                {
                    // Update all sensor data and convert to ASCII bytes.
                    var data = System.Text.Encoding.ASCII.GetBytes(machine.RefreshAndSerialise().Trim() + "\r\n");
                    
                    serialPort.WriteTimeout = 500;
                    await serialPort.BaseStream.WriteAsync(data, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                
                // Force-close port for next loop iteration.
                serialPort?.Close();
                serialPort?.Dispose();
                serialPort = null;
                
                // Short delay before attempting reconnection
                await Task.Delay(5000, stoppingToken);
            }
            
            // Wait for duration defined in settings before next update.
            await Task.Delay((int)machine.Settings.PollingRate!, stoppingToken);
        }
    }
}