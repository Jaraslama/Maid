using System;
using System.IO;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static string LogFolder = "C:\\ProgramData\\Maiden";
    static string LogFile = "C:\\ProgramData\\Maiden\\history.json";
    static string SizesFile = "C:\\ProgramData\\Maiden\\sizes.json";

    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--scan")
        {
            ScanFolderSizes();
            return;
        }

        string userTemp = Path.GetTempPath();
        string winLogs = "C:\\Windows\\Logs";
        string winTemp = "C:\\Windows\\Temp";
        string updateCache = "C:\\Windows\\SoftwareDistribution\\Download";

        long totalBytesFreed = 0;

        totalBytesFreed += CleanFolder(userTemp);
        totalBytesFreed += CleanFolder(winLogs);
        totalBytesFreed += CleanFolder(winTemp);
        totalBytesFreed += CleanUpdateCache(updateCache);

        LogRun(totalBytesFreed);
    }

    static void ScanFolderSizes()
    {
        var sizes = new Dictionary<string, long>
        {
            { "UserTemp", GetDirectorySize(Path.GetTempPath()) },
            { "WinLogs", GetDirectorySize("C:\\Windows\\Logs") },
            { "WinTemp", GetDirectorySize("C:\\Windows\\Temp") },
            { "UpdateCache", GetDirectorySize("C:\\Windows\\SoftwareDistribution\\Download") }
        };

        Directory.CreateDirectory(LogFolder);
        string json = JsonSerializer.Serialize(sizes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SizesFile, json);
    }

    static long CleanFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath)) return 0;

        long bytesFreed = 0;

        string[] files = Directory.GetFiles(folderPath);
        string[] dirs = Directory.GetDirectories(folderPath);

        foreach (string file in files)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(file);

                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(file, attributes & ~FileAttributes.ReadOnly);
                }

                long size = new FileInfo(file).Length;
                File.Delete(file);
                bytesFreed += size;
            }
            catch
            {
                // skip - locked or access denied
            }
        }

        foreach (string dir in dirs)
        {
            try
            {
                long dirSize = GetDirectorySize(dir);
                Directory.Delete(dir, true);
                bytesFreed += dirSize;
            }
            catch
            {
                // skip - locked or access denied
            }
        }

        return bytesFreed;
    }

    static long GetDirectorySize(string folderPath)
    {
        long size = 0;
        try
        {
            foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                try { size += new FileInfo(file).Length; } catch { }
            }
        }
        catch { }
        return size;
    }

    static long CleanUpdateCache(string folderPath)
    {
        if (!Directory.Exists(folderPath)) return 0;

        bool serviceStopped = false;

        try
        {
            using (ServiceController wu = new ServiceController("wuauserv"))
            {
                if (wu.Status == ServiceControllerStatus.Running)
                {
                    wu.Stop();
                    wu.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
                    serviceStopped = true;
                }
            }
        }
        catch
        {
            // couldn't stop the service (maybe not admin) - continue anyway
        }

        TakeOwnership(folderPath);
        long bytesFreed = CleanFolder(folderPath);

        if (serviceStopped)
        {
            try
            {
                using (ServiceController wu = new ServiceController("wuauserv"))
                {
                    wu.Start();
                }
            }
            catch
            {
                // ignore - user may need to start it manually
            }
        }

        return bytesFreed;
    }

    static void TakeOwnership(string folderPath)
    {
        RunHidden("takeown.exe", $"/f \"{folderPath}\" /r /d y");
        RunHidden("icacls.exe", $"\"{folderPath}\" /grant Administrators:F /t /c");
    }

    static void RunHidden(string fileName, string arguments)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
            }
        }
        catch
        {
            // ignore
        }
    }

    static void LogRun(long bytesFreed)
    {
        try
        {
            Directory.CreateDirectory(LogFolder);

            List<CleanupEntry> history = new List<CleanupEntry>();

            if (File.Exists(LogFile))
            {
                string existingJson = File.ReadAllText(LogFile);
                history = JsonSerializer.Deserialize<List<CleanupEntry>>(existingJson) ?? new List<CleanupEntry>();
            }

            history.Add(new CleanupEntry
            {
                Timestamp = DateTime.Now,
                BytesFreed = bytesFreed
            });

            string newJson = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(LogFile, newJson);
        }
        catch
        {
            // if logging fails, don't crash the whole app over it
        }
    }
}

class CleanupEntry
{
    public DateTime Timestamp { get; set; }
    public long BytesFreed { get; set; }
}