namespace SysMonService.Interfaces;

/// <summary>
/// Contract for objects that can be converted to and from a serialisable format.
/// Used to prepare hardware telemetry for transmission over the serial port.
/// </summary>
public interface ISerialisable<T>
{
    /// <summary>
    /// Converts the current state of the object into a string representation.
    /// Produces the formatted packet sent to the microcontroller.
    /// </summary>
    /// <returns>A string representation of the object's data.</returns>
    string Serialise();
    
    /// <summary>
    /// Reconstructs an object of type <typeparamref name="T"/> from a source.
    /// Useful if the microcontroller sends data back to the service.
    /// </summary>
    /// <returns>A new instance of <typeparamref name="T"/> or <c>null</c>
    /// if deserialisation fails.</returns>
    T?  Deserialise();
}