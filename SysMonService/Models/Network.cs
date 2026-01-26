using SysMonService.Interfaces;

namespace SysMonService.Models;

public class Network(IHardwareProvider hardwareProvider) : SimpleSerialiser<Network>, INetwork
{
    public double? Sent { get; set; }
    
    public double? Received { get; set; }

    public void Refresh()
    {
        this.Sent = hardwareProvider.GetNetworkSent();
        this.Received = hardwareProvider.GetNetworkReceived();
    }
}