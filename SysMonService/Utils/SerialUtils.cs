using System.IO.Ports;

namespace SysMonService.Utils;

/// <summary>
/// Provides utility methods for managing serial port connections.
/// </summary>
public static class SerialUtils
{
    /// <summary>
    /// Attempts to initialize and open a serial port. 
    /// This method will block and retry indefinitely until a connection is successfully established.
    /// </summary>
    /// <param name="portName">The name of the COM port (e.g., "COM3").</param>
    /// <param name="baudRate">The transmission speed (e.g., 9600).</param>
    /// <param name="parity">the parity-checking protocol.</param>
    /// <param name="dataBits">The standard length of data bits per byte.</param>
    /// <param name="stopBits">The standard number of stop bits per byte.</param>
    /// <returns>An initialized and open <see cref="SerialPort"/> instance.</returns>
    /// <remarks>
    /// Note: This method implements an infinite loop. It is intended to be used in 
    /// background scenarios where the service must wait for hardware to be plugged in.
    /// </remarks>
    public static async Task<SerialPort?> GetOpenSerialPort(
        string portName,
        int baudRate,
        Parity parity,
        int dataBits,
        StopBits stopBits)
    {
        return await Task.Run(() =>
        {
            // Initialise the port with the specified hardware parameters
            var serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            
            // DTR (Data Terminal Ready) and RTS (Request to Send) are frequently 
            // required to be true for microcontrollers to communicate or reset upon connection.
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;

            try
            {
                serialPort.Open();
                return serialPort;
            }
            catch (Exception ex)
            {
                // Log the failure to the debug console and wait before retrying.
                // This prevents high CPU usage during the retry loop.
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                // Dispose failed port object and release system resources.
                serialPort?.Dispose();

                // small delay after disposing serial open attempt.
                Thread.Sleep(50);

                return serialPort;
            }
        });
    }
}