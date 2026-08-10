using System.Threading;
using System.Threading.Tasks;
using Optimus.Diagnostics.Core.Models;

namespace Optimus.Diagnostics.Core.Interfaces;

/// <summary>
/// Represents a scanner capable of retrieving specific system information or health metrics.
/// </summary>
public interface IDiagnosticScanner<TResult>
{
    string ScannerName { get; }

    /// <summary>
    /// Executes the diagnostic scan asynchronously.
    /// </summary>
    Task<DiagnosticResult<TResult>> ScanAsync(CancellationToken cancellationToken = default);
}