using System.Runtime.Serialization;
using SysMonService.Interfaces;

namespace SysMonService.Models;

public class System(IHardwareProvider hardwareProvider) : SimpleSerialiser<System>
{
    public Processor? Processor { get; set; } = new Processor(hardwareProvider);

    public Memory? Memory { get; set; } = new Memory(hardwareProvider);
    
    public Graphics? Graphics { get; set; } = new  Graphics(hardwareProvider);
    
    public Network? Network { get; set; } = new  Network(hardwareProvider);
}