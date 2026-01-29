# ESP32-S3 System Monitor (N16R8)

A background telemetry system for Windows that streams hardware statistics (CPU, GPU, RAM) to an ESP32-S3 display using JSON over Serial.

---

## 1. Hardware & Firmware Setup

### 1.1 Flash & Partition Configuration
Because the ESP32-S3 N16R8 uses 16MB Flash and OPI PSRAM, standard partition tables will cause boot loops. You must flash the following components to these specific addresses:

| Component | Offset | Description |
| :--- | :--- | :--- |
| bootloader.bin | 0x0000 | Hardware initialization |
| partitions.bin | 0x8000 | Custom 16MB partition table |
| firmware.bin | 0x10000 | The main application logic |

---

## 2. Service Configuration (appsettings.json)

The service will not start without this file. Create a file named appsettings.json in the same folder as SysMonService.exe with the following structure:

{
  "MachineSettings": {
    "ComPort": "COM3",
    "PollingRate": 1000
  }
}

* ComPort: The COM port of your ESP32 (Check Device Manager).
* PollingRate: Update frequency in milliseconds (default 2000).

---

## 3. Windows Service Installation

This application runs as a Windows Service to allow it to start automatically before login and recover if the USB is unplugged.

### 3.1 PowerShell Installation Commands
Open PowerShell as Administrator and run these commands to install and configure the service:

# 1. Install the Service
# Change the path below to where your .exe is located
New-Service -Name "SysMonService" `
            -BinaryPathName "C:\Path\To\Your\Folder\SysMonService.exe" `
            -DisplayName "ESP32 System Monitor" `
            -StartupType Automatic

2 3. Start the Service
Start-Service -Name "SysMonService"

---

## 4. Troubleshooting

### 4.1 Error 1053 (Service Start Timeout)
If the service fails to start within 30 seconds:
1. NuGet Package: Ensure Microsoft.Extensions.Hosting.WindowsServices (v9.0.x) is installed.
2. Program.cs: Ensure builder.Services.AddWindowsService() is called before builder.Build().
3. Config Location: Verify appsettings.json is in the same folder as the .exe.
4. Logs: Check Event Viewer -> Windows Logs -> Application for .NET Runtime errors.

### 4.2 "Marked for Deletion"
If you cannot delete or reinstall the service:
1. Close services.msc.
2. Close Task Manager.
3. Run "taskkill /F /IM SysMonService.exe" in Admin PowerShell.
4. Restart Windows (required to clear the service handle lock).

### 4.3 ESP32 Reset Loops
If the ESP32 screen flashes or resets every time the service starts:
* In SerialUtils.cs, set DtrEnable = false and RtsEnable = false. This prevents the S3's auto-reset circuit from triggering on connection.

---

## 5. Service Management Tools
- Check Status: Get-Service -Name "SysMonService"
- Stop Service: Stop-Service -Name "SysMonService"
- Remove Service: sc.exe delete "SysMonService"

## 🛠️ Hardware & Case
You can find the 3D printable case for this project here:
[![Printables](https://media.printables.com/media/prints/61230ae5-ea7e-46a2-b0e0-0fed99b89bb7/images/11870798_6469ae21-9fa4-4fc7-bc44-fa27b2a7cd6c_3a876afc-fb1d-4a57-b5c8-06769f80e5d9/thumbs/inside/1920x1440/jpg/pxl_20260129_133507013mp.webp)](https://www.printables.com/model/1576094-esp32-system-monitor-case)
