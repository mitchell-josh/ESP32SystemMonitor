namespace SysMonService.Interfaces;

/// <summary>
/// Represents a network interface card (NIC) and its data throughput telemetry.
/// Inherits base hardware properties from <see cref="IHardware"/>
/// </summary>
public interface INetwork : IHardware
{
    /// <summary>
    /// Gets or sets the current outgoing data rate (Upload speed) in Megabytes per second.
    /// </summary>
    decimal? Sent { get; set; }
    
    /// <summary>
    /// Gets or sets the current incoming data rate (Download speed) in Megabytes per second.
    /// </summary>
    decimal? Received { get; set; }
}