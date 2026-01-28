using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

public class Network(IHardwareProvider hardwareProvider) : SimpleSerialiser<Network>, INetwork
{
    public decimal? Sent { get; set; }
    
    public decimal? Received { get; set; }

    public void Refresh()
    {
        this.Sent = hardwareProvider.GetNetworkSent().BytesToMbps().Round2();
        this.Received = hardwareProvider.GetNetworkReceived().BytesToMbps().Round2();
    }
}