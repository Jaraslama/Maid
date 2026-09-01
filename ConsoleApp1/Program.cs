using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.ServiceProcess;

class Program
{
    static void Main()
    {
        string userTemp = Path.GetTempPath();
        string winLogs = "C:\\Windows\\Logs";
        string winTemp = "C:\\Windows\\Temp";
        string updateCache = "C:\\Windows\\SoftwareDistribution\\Download";

        CleanFolder(userTemp);
        CleanFolder(winLogs);
        CleanFolder(winTemp);
        CleanUpdateCache(updateCache);

        MessageBox.Show("Hotovo! Počítač byl uklizen.", "Úklid dokončen");
    }

    static void CleanFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath)) return;

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

                File.Delete(file);
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
                Directory.Delete(dir, true);
            }
            catch
            {
                // skip - locked or access denied
            }
        }
    }

    static void CleanUpdateCache(string folderPath)
    {
        if (!Directory.Exists(folderPath)) return;

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
        CleanFolder(folderPath);

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
}