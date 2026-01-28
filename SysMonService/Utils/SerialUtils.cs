using System.IO.Ports;

namespace SysMonService.Utils;

public static class SerialUtils
{
    public static SerialPort GetOpenSerialPort(
        string portName,
        int baudRate,
        Parity parity,
        int dataBits,
        StopBits stopBits)
    {
        while (true)
        {
            var serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;

            try
            {
                serialPort.Open();
                return serialPort;
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}