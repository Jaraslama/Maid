using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static string LogFolder = "C:\\ProgramData\\Maiden";
    static string LogFile = "C:\\ProgramData\\Maiden\\history.json";
    static string SizesFile = "C:\\ProgramData\\Maiden\\sizes.json";

    static int Main(string[] args)
    {
        ProgressReporter? progress = null;
        try
        {
            if (args.Length > 0 && args[0] == "--scan")
            {
                ScanFolderSizes();
                return 0;
            }

            if (args.Length == 2 && args[0] == "--progress-file")
            {
                progress = new ProgressReporter(args[1]);
                progress.Report(0, "Progress.Starting");
            }

            string userTemp = Path.GetTempPath();
            string winLogs = "C:\\Windows\\Logs";
            string winTemp = "C:\\Windows\\Temp";

            long totalBytesFreed = 0;

            totalBytesFreed += CleanFolder(userTemp, progress, 0, 30, "Progress.UserTemp");
            totalBytesFreed += CleanFolder(winLogs, progress, 30, 60, "Progress.WinLogs");
            totalBytesFreed += CleanFolder(winTemp, progress, 60, 96, "Progress.WinTemp");

            progress?.Report(98, "Progress.SavingHistory");
            LogRun(totalBytesFreed);
            progress?.Report(100, "Progress.Completed");
            return 0;
        }
        catch (Exception ex)
        {
            progress?.ReportFailure(ex);
            return 1;
        }
    }

    static void ScanFolderSizes()
    {
        var sizes = new Dictionary<string, long>
        {
            { "UserTemp", GetDirectorySize(Path.GetTempPath()) },
            { "WinLogs", GetDirectorySize("C:\\Windows\\Logs") },
            { "WinTemp", GetDirectorySize("C:\\Windows\\Temp") }
        };

        Directory.CreateDirectory(LogFolder);
        string json = JsonSerializer.Serialize(sizes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SizesFile, json);
    }

    static long CleanFolder(string folderPath, ProgressReporter? progress, int startPercent, int endPercent, string stage)
    {
        progress?.Report(startPercent, stage);
        if (!Directory.Exists(folderPath))
        {
            progress?.Report(endPercent, "Progress.CleaningItems");
            return 0;
        }

        long bytesFreed = 0;
        var files = new List<string>();
        var dirs = new List<string>();
        var pendingDirectories = new Stack<string>();
        pendingDirectories.Push(folderPath);
        int scannedDirectories = 0;

        while (pendingDirectories.Count > 0)
        {
            string directory = pendingDirectories.Pop();
            scannedDirectories++;
            progress?.Report(Math.Min(startPercent + 8, startPercent + scannedDirectories), "Progress.Scanning");

            try
            {
                files.AddRange(Directory.GetFiles(directory));
                string[] childDirectories = Directory.GetDirectories(directory);
                dirs.AddRange(childDirectories);

                foreach (string childDirectory in childDirectories)
                {
                    pendingDirectories.Push(childDirectory);
                }
            }
            catch
            {
                // Skip folders that are locked or inaccessible.
            }
        }

        int totalItems = files.Count + dirs.Count;
        int completedItems = 0;
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

            completedItems++;
            progress?.Report(GetCleanupPercent(startPercent, endPercent, completedItems, totalItems), "Progress.CleaningItems");
        }

        for (int i = dirs.Count - 1; i >= 0; i--)
        {
            string dir = dirs[i];
            try
            {
                Directory.Delete(dir);
            }
            catch
            {
                // skip - locked or access denied
            }

            completedItems++;
            progress?.Report(GetCleanupPercent(startPercent, endPercent, completedItems, totalItems), "Progress.CleaningItems");
        }

        progress?.Report(endPercent, "Progress.CleaningItems");
        return bytesFreed;
    }

    static int GetCleanupPercent(int startPercent, int endPercent, int completedItems, int totalItems)
    {
        int deletionStart = Math.Min(startPercent + 8, endPercent);
        if (totalItems == 0) return endPercent;

        return deletionStart + (int)((endPercent - deletionStart) * (completedItems / (double)totalItems));
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

    class ProgressReporter
    {
        private readonly string _filePath;
        private int _lastPercent = -1;
        private string? _lastStatus;
        private bool _enabled = true;

        public ProgressReporter(string filePath)
        {
            _filePath = filePath;
        }

        public void Report(int percent, string status)
        {
            if (!_enabled || (percent == _lastPercent && status == _lastStatus)) return;

            if (!TryWriteEntry(new ProgressEntry { Percent = percent, Status = status })) return;
            _lastPercent = percent;
            _lastStatus = status;
        }

        public void ReportFailure(Exception exception)
        {
            TryWriteEntry(new ProgressEntry
            {
                Percent = _lastPercent < 0 ? 0 : _lastPercent,
                Status = "Progress.Failed",
                Error = exception.ToString()
            });
        }

        private bool TryWriteEntry(ProgressEntry entry)
        {
            if (!_enabled) return false;

            try
            {
                string json = JsonSerializer.Serialize(entry);
                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (IOException)
            {
                _enabled = false;
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                _enabled = false;
                return false;
            }
        }
    }

    class ProgressEntry
    {
        public int Percent { get; set; }
        public string Status { get; set; } = "";
        public string? Error { get; set; }
    }
}

class CleanupEntry
{
    public DateTime Timestamp { get; set; }
    public long BytesFreed { get; set; }
}