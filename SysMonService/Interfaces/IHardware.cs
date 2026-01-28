namespace SysMonService.Interfaces;

/// <summary>
/// Base contract for all monitorable hardware components.
/// Ensures that every hardware entity provides a mechanism to synchronize its local properties
/// with up-to-date sensor readings.
/// </summary>
public interface IHardware
{
    /// <summary>
    /// Triggers a fresh poll of the hardware sensors.
    /// This method should be called immediately before accessing telemetry properties to ensure the data is up-to-date
    /// and not cached.
    /// </summary>
    void Refresh();
}