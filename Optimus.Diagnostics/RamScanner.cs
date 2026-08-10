using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Management.Infrastructure;
using Optimus.Core.Enums;
using Optimus.Core.Logging;
using Optimus.Diagnostics.Core.Interfaces;
using Optimus.Diagnostics.Core.Models;
using Optimus.Diagnostics.Hardware.Interop;
using Optimus.Diagnostics.Hardware.Models;

namespace Optimus.Diagnostics.Hardware.Scanners;

public sealed class RamScanner : IDiagnosticScanner<RamInfo>
{
    private readonly ILogger _logger;
    private readonly INativeMemoryInterop _nativeInterop;

    public string ScannerName => "RAM Hardware Scanner";

    public RamScanner(ILogger logger, INativeMemoryInterop nativeInterop)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _nativeInterop = nativeInterop ?? throw new ArgumentNullException(nameof(nativeInterop));
    }

    public async Task<DiagnosticResult<RamInfo>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var memStatus = _nativeInterop.GetMemoryStatus();
            var (speedMhz, manufacturer) = await Task.Run(() => GetRamHardwareInfoFromCim(cancellationToken), cancellationToken);

            var ramInfo = new RamInfo(
                TotalPhysicalMemoryBytes: memStatus.ullTotalPhys,
                AvailablePhysicalMemoryBytes: memStatus.ullAvailPhys,
                TotalPageFileBytes: memStatus.ullTotalPageFile,
                AvailablePageFileBytes: memStatus.ullAvailPageFile,
                MemoryLoadPercentage: memStatus.dwMemoryLoad,
                SpeedMHz: speedMhz,
                Manufacturer: manufacturer
            );

            stopwatch.Stop();
            return DiagnosticResult<RamInfo>.Success(ramInfo, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            await _logger.LogAsync(LogLevel.Error, "Failed to scan RAM.", ex);
            return DiagnosticResult<RamInfo>.Failure("RAM scan failed.", ex, stopwatch.Elapsed);
        }
    }

    private static (uint Speed, string Manufacturer) GetRamHardwareInfoFromCim(CancellationToken cancellationToken)
    {
        using var session = CimSession.Create(null);
        var query = session.QueryInstances(@"root\cimv2", "WQL", "SELECT Speed, Manufacturer FROM Win32_PhysicalMemory");

        var modules = query.ToList();
        if (!modules.Any()) return (0, "Unknown");

        uint speed = Convert.ToUInt32(modules.First().CimInstanceProperties["Speed"].Value ?? 0);
        string manufacturer = modules.First().CimInstanceProperties["Manufacturer"].Value?.ToString()?.Trim() ?? "Unknown";

        return (speed, manufacturer);
    }
}