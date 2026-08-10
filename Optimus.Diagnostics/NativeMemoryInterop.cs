using System;
using System.Runtime.InteropServices;

namespace Optimus.Diagnostics.Hardware.Interop;

public static partial class NativeMethods
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public void Init()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
}

public interface INativeMemoryInterop
{
    NativeMethods.MEMORYSTATUSEX GetMemoryStatus();
}

public sealed class NativeMemoryInterop : INativeMemoryInterop
{
    public NativeMethods.MEMORYSTATUSEX GetMemoryStatus()
    {
        var memStatus = new NativeMethods.MEMORYSTATUSEX();
        memStatus.Init();
        if (!NativeMethods.GlobalMemoryStatusEx(ref memStatus))
        {
            var errorCode = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"GlobalMemoryStatusEx failed with error code: {errorCode}");
        }
        return memStatus;
    }
}