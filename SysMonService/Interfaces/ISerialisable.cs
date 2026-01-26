namespace SysMonService.Interfaces;

public interface ISerialisable<T>
{
    string Serialise();
    
    T?  Deserialise();
}