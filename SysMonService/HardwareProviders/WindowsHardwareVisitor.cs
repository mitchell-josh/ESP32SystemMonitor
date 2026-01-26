using LibreHardwareMonitor.Hardware;

namespace SysMonService.HardwareProviders;

public class WindowsHardwareVisitor : IVisitor
{
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }

    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();

        foreach (var subHardware in hardware.SubHardware)
        {
            subHardware.Accept(this);
        }

        foreach (var sensor in hardware.Sensors)
        {
            sensor.Accept(this);
        }
    }

    public void VisitSensor(ISensor sensor)
    {
        return;
    }

    public void VisitParameter(IParameter parameter)
    {
        return;
    }
}