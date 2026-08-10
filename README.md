
#  Optimus Enterprise Suite

**Optimus Enterprise Suite** is an advanced, enterprise-grade system diagnostic, telemetry, and optimization tool built for Windows. Designed with a strict local-first architecture, it provides absolute control over hardware resources and OS integrity without relying on cloud analytics or external APIs.

The core of the application is the **Optimus Neural Co-Pilot**, a built-in NLP (Natural Language Processing) intent engine that parses user commands in real-time to execute deep system diagnostics, cleanups, and registry optimizations with zero latency.

## 🚀 Core Features

*   **Live Hardware Telemetry:** Real-time, deep-level monitoring of CPU, RAM, GPU, Disk, and Network stack (ping, speeds, handle counts, and active threads).
*   **Neural Co-Pilot (Local Intent Engine):** A holographic command-line interface that understands natural language triggers to execute tasks (e.g., polling sensors, flushing DNS, cleaning temp files).
*   **Process Trust Monitor:** Live tracking of system processes, evaluating their memory footprint and categorizing their risk level (Core, Safe, Heavy).
*   **1-Click Secure Optimize:** Safely applies targeted Windows Registry tweaks designed to reduce input lag and free up resources (e.g., disabling Xbox Game Bar telemetry and Fullscreen Optimizations).
*   **System Repair Center:** Executes native Windows repair tools (`sfc /scannow`, `DISM`, `ipconfig /flushdns`) entirely in the background, displaying real-time success logs and confidence scores.
*   **Pre-Flight Safety Checks:** Autonomous optimization plans require user validation via a transparent checklist before altering OS parameters.
*   **Zero-Cloud Architecture:** 100% offline. No data leaves the machine.

## 🛠️ Tech Stack & Architecture

*   **Framework:** .NET Core / Windows Presentation Foundation (WPF)
*   **Architecture Pattern:** Strict MVVM (Model-View-ViewModel)
*   **Libraries:** `CommunityToolkit.Mvvm` (for efficient ObservableObjects and RelayCommands)
*   **UI/UX:** Custom Glassmorphism design, native drop-shadow neon effects, and highly responsive XAML layouts.

## ⚡ How it Works (Under the Hood)

Unlike standard optimization tools, Optimus operates by interfacing directly with the Windows API and Registry (`Microsoft.Win32`). 
The **Neural Co-Pilot** bypasses traditional LLM cloud constraints by using deterministic local intent parsing. When a user inputs a command, the engine cross-references the required system variables, executes native background processes (via `ProcessStartInfo`), and returns a simulated typewriter-effect audit stream. 

## 🛡️ Disclaimer
This application modifies specific Windows Registry keys (`HKEY_CURRENT_USER\System\GameConfigStore`) to optimize system performance for heavy rendering and gaming. It also executes Administrative commands (`sfc`, `dism`). Please ensure you run the application with appropriate permissions.
