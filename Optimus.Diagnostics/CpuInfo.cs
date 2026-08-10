namespace Optimus.Diagnostics.Hardware.Models;

public sealed record CpuInfo(
    string Name,
    string Manufacturer,
    uint NumberOfCores,
    uint NumberOfLogicalProcessors,
    uint MaxClockSpeedMHz,
    uint L2CacheSizeKB,
    uint L3CacheSizeKB,
    string VirtualizationFirmwareEnabled
);