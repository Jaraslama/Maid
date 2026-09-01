# Cleanup App

A small Windows utility that clears out common junk/temp locations and shows a toast notification with a summary when it's done.

## What it cleans

- `C:\Windows\Logs`
- `C:\Windows\Temp`
- Your user temp folder (`%TEMP%`)

Files and folders that are locked or in use by another program are skipped automatically, and the summary will tell you how many were skipped.

## Requirements

- Windows 10/11
- [.NET 10 Runtime](https://dotnet.microsoft.com/download) installed on your machine

## Download

Grab the latest release from the [Releases](../../releases) page. Two builds are provided:

- `CleanupApp-x64.exe` — for 64-bit systems
- `CleanupApp-x86.exe` — for 32-bit systems

## Usage

Just run the `.exe`. It automatically installs as a scheduled task scheduled to run every time you log in.

> **Note:** Cleaning `C:\Windows\Logs` and `C:\Windows\Temp` may require running the app as Administrator, since these are system folders.

## Disclaimer

This tool permanently deletes files , i am not responsible for any damages done due to poor setup , Use at your own risk.
