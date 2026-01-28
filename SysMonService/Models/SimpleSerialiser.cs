using System.Text.Json;
using SysMonService.Interfaces;

namespace SysMonService.Models;

public abstract class SimpleSerialiser<T> : ISerialisable<T>
{
    public string Serialise() => JsonSerializer.Serialize(this, this.GetType());

    public T? Deserialise() => JsonSerializer.Deserialize<T>(Serialise());
}