namespace Optimus.Diagnostics.Hardware.Models;

public sealed record RamInfo(
    ulong TotalPhysicalMemoryBytes,
    ulong AvailablePhysicalMemoryBytes,
    ulong TotalPageFileBytes,
    ulong AvailablePageFileBytes,
    uint MemoryLoadPercentage,
    uint SpeedMHz,
    string Manufacturer
);