using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.Uwp.Notifications;

class Program
{
    [DllImport("shell32.dll", SetLastError = true)]
    static extern void SetCurrentProcessExplicitAppUserModelID(
        [MarshalAs(UnmanagedType.LPWStr)] string AppID);

    static void Main()
    {
        SetCurrentProcessExplicitAppUserModelID("MyCleanupApp");

        string userTemp = Path.GetTempPath();
        string winLogs = "C:\\Windows\\Logs";
        string winTemp = "C:\\Windows\\Temp";

        StringBuilder summary = new StringBuilder();

        summary.AppendLine(CleanFolder(userTemp));
        summary.AppendLine(CleanFolder(winLogs));
        summary.AppendLine(CleanFolder(winTemp));

        ShowToast("Cleanup finished", summary.ToString());
    }

    static string CleanFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            return $"{folderPath}: folder not found, skipped.";
        }

        string[] files = Directory.GetFiles(folderPath);
        string[] dirs = Directory.GetDirectories(folderPath);

        int deletedFiles = 0, skippedFiles = 0;
        int deletedDirs = 0, skippedDirs = 0;

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
                deletedFiles++;
            }
            catch
            {
                skippedFiles++;
            }
        }

        foreach (string dir in dirs)
        {
            try
            {
                Directory.Delete(dir, true);
                deletedDirs++;
            }
            catch
            {
                skippedDirs++;
            }
        }

        return $"{folderPath}: {deletedFiles} files, {deletedDirs} folders deleted " +
               $"({skippedFiles} files, {skippedDirs} folders skipped).";
    }

    static void ShowToast(string title, string message)
    {
        new ToastContentBuilder()
            .AddText(title)
            .AddText(message)
            .Show();
    }
}