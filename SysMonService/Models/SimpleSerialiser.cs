using System.Text.Json;
using SysMonService.Interfaces;

namespace SysMonService.Models;

public class SimpleSerialiser<T> : ISerialisable<T>
{
    public string Serialise() => JsonSerializer.Serialize(this);

    public T? Deserialise() => JsonSerializer.Deserialize<T>(Serialise());
}