using System.Runtime.Serialization;
using SysMonService.Interfaces;
using SysMonService.Utils;

namespace SysMonService.Models;

public class System(IHardwareProvider hardwareProvider, Settings settings) : SimpleSerialiser<System>
{
    public Settings Settings => settings;
    
    public Processor? Processor { get; set; } = new Processor(hardwareProvider);

    public Memory? Memory { get; set; } = new Memory(hardwareProvider);
    
    public Graphics? Graphics { get; set; } = new  Graphics(hardwareProvider);
    
    public Network? Network { get; set; } = new  Network(hardwareProvider);

    public void Refresh()
    {
        this.Processor?.Refresh();
        this.Memory?.Refresh();
        this.Graphics?.Refresh();
        this.Network?.Refresh();
    }

    public string RefreshAndSerialise()
    {
        this.Refresh();
        return this.Serialise();
    }
}