using System;

namespace Optimus.Diagnostics.Core.Models;

/// <summary>
/// A standardized wrapper for all diagnostic operations, encapsulating success state, data, and timing.
/// </summary>
public sealed class DiagnosticResult<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public Exception? Exception { get; }
    public TimeSpan ExecutionTime { get; }

    private DiagnosticResult(bool isSuccess, T? data, string? errorMessage, Exception? exception, TimeSpan executionTime)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        Exception = exception;
        ExecutionTime = executionTime;
    }

    public static DiagnosticResult<T> Success(T data, TimeSpan executionTime) =>
        new(true, data, null, null, executionTime);

    public static DiagnosticResult<T> Failure(string errorMessage, Exception? exception, TimeSpan executionTime) =>
        new(false, default, errorMessage, exception, executionTime);
}