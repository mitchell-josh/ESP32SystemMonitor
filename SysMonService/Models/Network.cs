using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

/// <summary>
/// Represents a network interface card (NIC) and its data throughput telemetry.
/// Inherits base hardware properties from <see cref="IHardware"/>
/// </summary>
public class Network(IHardwareProvider hardwareProvider) : SimpleSerialiser<Network>, INetwork
{
    /// <summary>
    /// Gets or sets the current outgoing data rate (Upload speed) in Megabytes per second.
    /// </summary>
    public decimal? Sent { get; set; }
    
    /// <summary>
    /// Gets or sets the current incoming data rate (Download speed) in Megabytes per second.
    /// </summary>
    public decimal? Received { get; set; }

    /// <summary>
    /// Triggers a fresh poll of the network interface cards (NIC) hardware sensors.
    /// This method should be called immediately before accessing telemetry properties to ensure the data is up-to-date
    /// and not cached.
    /// </summary>
    public void Refresh()
    {
        this.Sent = hardwareProvider.GetNetworkSent().BytesToMbps().Round2();
        this.Received = hardwareProvider.GetNetworkReceived().BytesToMbps().Round2();
    }
}