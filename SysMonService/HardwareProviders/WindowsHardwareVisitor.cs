using LibreHardwareMonitor.Hardware;

namespace SysMonService.HardwareProviders;

/// <summary>
/// Implements Visitor to traverse the hardware hierarchy.
/// Triggers internal updates for all hardware components and sensors so that real-time data is available
/// for retrieval.
/// </summary>
public class WindowsHardwareVisitor : IVisitor
{
    /// <summary>
    /// Entry point for the visitor. Starts the traversal of the entire system.
    /// </summary>
    /// <param name="computer">The root computer object to traverse.</param>
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }

    /// <summary>
    /// Updates the specific hardware component and recursively 
    /// visits all sub-hardware and sensors attached to it.
    /// </summary>
    /// <param name="hardware">The hardware component currently being visited.</param>
    public void VisitHardware(IHardware hardware)
    {
        // Triggers driver to poll hardware for new values (clocks, temperatures, etc.)
        hardware.Update();

        // Recursively visit sub-hardware.
        foreach (var subHardware in hardware.SubHardware)
        {
            subHardware.Accept(this);
        }

        // Visit sensors to ensure internal state is updated.
        foreach (var sensor in hardware.Sensors)
        {
            sensor.Accept(this);
        }
    }

    /// <summary>
    /// Required by IVisitor, but sensors are updated via the Hardware's Update call.
    /// </summary>
    public void VisitSensor(ISensor sensor)
    {
        return;
    }

    /// <summary>
    /// Required by IVisitor, used for hardware that supports configurable parameters.
    /// </summary>
    public void VisitParameter(IParameter parameter)
    {
        return;
    }
}