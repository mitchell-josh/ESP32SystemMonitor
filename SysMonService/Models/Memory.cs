using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

/// <summary>
/// Represents the physical memory (RAM) and it's current utilisation state.
/// Inherits base hardware properties from <see cref="IHardware"/>
/// </summary>
public class Memory(IHardwareProvider hardwareProvider) : SimpleSerialiser<Memory>, IMemory
{
    /// <summary>
    /// Gets or sets the total amount of physical memory currently being utilised in Gigabytes.
    /// </summary>
    public decimal? UsedMemory { get; set; }
    
    /// <summary>
    /// Triggers a fresh poll of the memory (RAM) hardware sensors.
    /// This method should be called immediately before accessing telemetry properties to ensure the data is up-to-date
    /// and not cached.
    /// </summary>
    public void Refresh()
    {
        this.UsedMemory = hardwareProvider.GetUsedMemory().Round2();
    }
}