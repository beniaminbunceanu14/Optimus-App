using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optimus.Core.Enums;
using Optimus.Core.Logging;
using Optimus.Diagnostics.Core.Interfaces;
using Optimus.Diagnostics.Hardware.Interop;
using Optimus.Diagnostics.Hardware.Models;
using Optimus.Diagnostics.Hardware.Scanners;

namespace Optimus.Wpf;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Inregistrăm componentele construite de noi anterior
                services.AddSingleton<ILogger>(p => new AsyncFileLogger(AppDomain.CurrentDomain.BaseDirectory, LogLevel.Trace));

                services.AddSingleton<INativeMemoryInterop, NativeMemoryInterop>();
                services.AddTransient<IDiagnosticScanner<CpuInfo>, CpuScanner>();
                services.AddTransient<IDiagnosticScanner<RamInfo>, RamScanner>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host) { await _host.StopAsync(TimeSpan.FromSeconds(3)); }
        base.OnExit(e);
    }
}