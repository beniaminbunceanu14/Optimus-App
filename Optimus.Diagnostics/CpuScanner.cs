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
using Optimus.Diagnostics.Hardware.Models;

namespace Optimus.Diagnostics.Hardware.Scanners;

public sealed class CpuScanner : IDiagnosticScanner<CpuInfo>
{
    private readonly ILogger _logger;
    public string ScannerName => "CPU Hardware Scanner";

    public CpuScanner(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DiagnosticResult<CpuInfo>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _logger.LogAsync(LogLevel.Trace, "Starting CPU CIM scan.");

            var cpuInfo = await Task.Run(() => GetCpuInfoFromCim(cancellationToken), cancellationToken);

            stopwatch.Stop();
            await _logger.LogAsync(LogLevel.Information, $"CPU scan completed successfully in {stopwatch.ElapsedMilliseconds}ms.");

            return DiagnosticResult<CpuInfo>.Success(cpuInfo, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            await _logger.LogAsync(LogLevel.Error, "Error during CPU scan.", ex);
            return DiagnosticResult<CpuInfo>.Failure("Failed to query CPU info.", ex, stopwatch.Elapsed);
        }
    }

    private static CpuInfo GetCpuInfoFromCim(CancellationToken cancellationToken)
    {
        using var session = CimSession.Create(null);
        var queryProcessors = session.QueryInstances(@"root\cimv2", "WQL", "SELECT Name, Manufacturer, NumberOfCores, NumberOfLogicalProcessors, MaxClockSpeed, L2CacheSize, L3CacheSize, VirtualizationFirmwareEnabled FROM Win32_Processor");

        var processor = queryProcessors.FirstOrDefault() ?? throw new InvalidOperationException("No processor found.");

        return new CpuInfo(
            Name: processor.CimInstanceProperties["Name"].Value?.ToString()?.Trim() ?? "Unknown CPU",
            Manufacturer: processor.CimInstanceProperties["Manufacturer"].Value?.ToString()?.Trim() ?? "Unknown",
            NumberOfCores: Convert.ToUInt32(processor.CimInstanceProperties["NumberOfCores"].Value ?? 0),
            NumberOfLogicalProcessors: Convert.ToUInt32(processor.CimInstanceProperties["NumberOfLogicalProcessors"].Value ?? 0),
            MaxClockSpeedMHz: Convert.ToUInt32(processor.CimInstanceProperties["MaxClockSpeed"].Value ?? 0),
            L2CacheSizeKB: Convert.ToUInt32(processor.CimInstanceProperties["L2CacheSize"].Value ?? 0),
            L3CacheSizeKB: Convert.ToUInt32(processor.CimInstanceProperties["L3CacheSize"].Value ?? 0),
            VirtualizationFirmwareEnabled: processor.CimInstanceProperties["VirtualizationFirmwareEnabled"].Value?.ToString() ?? "Unknown"
        );
    }
}