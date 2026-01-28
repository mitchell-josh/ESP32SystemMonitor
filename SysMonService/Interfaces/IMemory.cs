namespace SysMonService.Interfaces;

/// <summary>
/// Represents the physical memory (RAM) and it's current utilisation state.
/// Inherits base hardware properties from <see cref="IHardware"/>
/// </summary>
public interface IMemory : IHardware
{
    /// <summary>
    /// Gets or sets the total amount of physical memory currently being utilised in Gigabytes.
    /// </summary>
    decimal? UsedMemory { get; set; }
}