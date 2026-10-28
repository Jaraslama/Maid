using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaidenDashboard
{
    public partial class DashboardForm : Form
    {
        static string LogFile = "C:\\ProgramData\\Maiden\\history.json";
        static string SizesFile = "C:\\ProgramData\\Maiden\\sizes.json";
        static string MaidExePath = Path.Combine(AppContext.BaseDirectory, "Maid.exe");

        public DashboardForm()
        {
            InitializeComponent();

            ApplyLocalization();

            // Load data on startup
            LoadHistory();
            LoadFolderSizes();
        }

        private void LoadHistory()
        {
            if (!File.Exists(LogFile)) return;

            try
            {
                string json = File.ReadAllText(LogFile);
                List<CleanupEntry> history = JsonSerializer.Deserialize<List<CleanupEntry>>(json) ?? new List<CleanupEntry>();

                listHistory.Items.Clear();
                long totalFreed = 0;

                foreach (var entry in history)
                {
                    var item = new ListViewItem(entry.Timestamp.ToString("g"));
                    item.SubItems.Add(FormatBytes(entry.BytesFreed));
                    listHistory.Items.Add(item);
                    totalFreed += entry.BytesFreed;
                }

                if (history.Count > 0)
                {
                    if (Localization.CurrentLanguage == Localization.Language.Czech)
                        lblLastRun.Text = $"⏱️  Poslední čištění: {history[history.Count - 1].Timestamp:g}";
                    else
                        lblLastRun.Text = $"⏱️  Last cleanup: {history[history.Count - 1].Timestamp:g}";
                }

                if (Localization.CurrentLanguage == Localization.Language.Czech)
                    lblTotalFreed.Text = $"💾 Celkem uvolněno: {FormatBytes(totalFreed)}";
                else
                    lblTotalFreed.Text = $"💾 Total freed: {FormatBytes(totalFreed)}";
            }
            catch
            {
                lblLastRun.Text = Localization.Get("Status.LastRunError");
            }
        }

        private void LoadFolderSizes()
        {
            listSizes.Items.Clear();

            if (!File.Exists(SizesFile))
            {
                var item = new ListViewItem(Localization.Get("Status.NoScanYet"));
                item.SubItems.Add("-");
                listSizes.Items.Add(item);
                return;
            }

            try
            {
                string json = File.ReadAllText(SizesFile);
                Dictionary<string, long> sizes = JsonSerializer.Deserialize<Dictionary<string, long>>(json) ?? new Dictionary<string, long>();

                foreach (var pair in sizes)
                {
                    if (pair.Key == "UpdateCache") continue;

                    string displayName = Localization.Get($"Folder.{pair.Key}");
                    var item = new ListViewItem(displayName);
                    item.SubItems.Add(FormatBytes(pair.Value));
                    listSizes.Items.Add(item);
                }
            }
            catch
            {
                var item = new ListViewItem(Localization.Get("Status.ErrorReading"));
                item.SubItems.Add("-");
                listSizes.Items.Add(item);
            }
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            try
            {
                await RunElevatedProcessAsync("--scan");
                LoadFolderSizes();
            }
            catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                // user clicked "No" on the UAC prompt - ignore
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private string FormatBytes(long bytes)
        {
            double mb = bytes / 1024.0 / 1024.0;
            if (mb < 1024) return mb.ToString("F1") + " MB";
            double gb = mb / 1024.0;
            return gb.ToString("F2") + " GB";
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }

        private async void Button1_Click(object? sender, EventArgs e)
        {
            string progressFile = Path.Combine(Path.GetTempPath(), $"maid-progress-{Guid.NewGuid():N}.json");
            bool cancelled = false;
            bool completed = false;
            cleanupProgressPanel.Visible = true;
            cleanupProgressBar.Style = ProgressBarStyle.Marquee;
            cleanupProgressBar.MarqueeAnimationSpeed = 30;
            SetCleanupProgress(0, "Progress.Starting");
            SetCleanupRunning(true);

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = MaidExePath,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                psi.ArgumentList.Add("--progress-file");
                psi.ArgumentList.Add(progressFile);

                using (var process = Process.Start(psi) ?? throw new InvalidOperationException("The cleanup process could not be started."))
                {
                    Task exitTask = process.WaitForExitAsync();
                    while (!exitTask.IsCompleted)
                    {
                        UpdateCleanupProgress(progressFile);
                        await Task.WhenAny(exitTask, Task.Delay(200));
                    }

                    await exitTask;
                    UpdateCleanupProgress(progressFile);
                    if (process.ExitCode != 0)
                    {
                        string? errorDetails = ReadCleanupError(progressFile);
                        string message = $"The cleanup process exited with code {process.ExitCode}.";
                        if (!string.IsNullOrWhiteSpace(errorDetails))
                        {
                            message += Environment.NewLine + Environment.NewLine + errorDetails;
                        }

                        throw new InvalidOperationException(message);
                    }
                }

                cleanupProgressBar.Style = ProgressBarStyle.Blocks;
                cleanupProgressBar.Value = 100;
                SetCleanupProgress(100, "Progress.Refreshing");
                await RunElevatedProcessAsync("--scan");

                LoadHistory();
                LoadFolderSizes();
                cleanupProgressBar.Style = ProgressBarStyle.Blocks;
                cleanupProgressBar.Value = 100;
                SetCleanupProgress(100, "Progress.Completed");
                completed = true;
            }
            catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                cancelled = true;
            }
            catch (Exception ex)
            {
                SetCleanupProgress(cleanupProgressBar.Value, "Progress.Failed");
                MessageBox.Show(ex.Message, Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    File.Delete(progressFile);
                    File.Delete(progressFile + ".tmp");
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message, Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (cancelled)
                {
                    cleanupProgressPanel.Visible = false;
                }
                else if (!completed && cleanupProgressBar.Value == 100)
                {
                    SetCleanupProgress(99, "Progress.Failed");
                }

                SetCleanupRunning(false);
            }
        }

        private async Task RunElevatedProcessAsync(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = MaidExePath,
                Arguments = arguments,
                UseShellExecute = true,
                Verb = "runas"
            };

            using (var process = Process.Start(psi) ?? throw new InvalidOperationException("The Maid process could not be started."))
            {
                await process.WaitForExitAsync();
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"The Maid process exited with code {process.ExitCode}.");
                }
            }
        }

        private void UpdateCleanupProgress(string progressFile)
        {
            if (!File.Exists(progressFile)) return;

            try
            {
                using var stream = new FileStream(progressFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                using var document = JsonDocument.Parse(stream);
                int percent = Math.Clamp(document.RootElement.GetProperty("Percent").GetInt32(), 0, 100);
                string status = document.RootElement.GetProperty("Status").GetString() ?? "Progress.Starting";
                cleanupProgressBar.Style = ProgressBarStyle.Blocks;
                SetCleanupProgress(percent, status);
            }
            catch (IOException)
            {
                ShowProgressUnavailable();
            }
            catch (UnauthorizedAccessException)
            {
                ShowProgressUnavailable();
            }
            catch (JsonException)
            {
                ShowProgressUnavailable();
            }
        }

        private static string? ReadCleanupError(string progressFile)
        {
            if (!File.Exists(progressFile)) return null;

            using var stream = new FileStream(progressFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var document = JsonDocument.Parse(stream);
            return document.RootElement.TryGetProperty("Error", out JsonElement error)
                ? error.GetString()
                : null;
        }

        private void SetCleanupProgress(int percent, string status)
        {
            if (cleanupProgressBar.Style == ProgressBarStyle.Blocks)
            {
                cleanupProgressBar.Value = Math.Clamp(percent, cleanupProgressBar.Minimum, cleanupProgressBar.Maximum);
                lblCleanupProgress.Text = $"{cleanupProgressBar.Value}% - {Localization.Get(status)}";
            }
            else
            {
                lblCleanupProgress.Text = Localization.Get("Progress.Running");
            }
        }

        private void ShowProgressUnavailable()
        {
            cleanupProgressBar.Style = ProgressBarStyle.Marquee;
            lblCleanupProgress.Text = Localization.Get("Progress.Running");
        }

        private void SetCleanupRunning(bool running)
        {
            btnRefresh.Enabled = !running;
            button1.Enabled = !running;
            btnClearHistory.Enabled = !running;
            menuStrip.Enabled = !running;
        }

        private void lblLastRun_Click(object sender, EventArgs e)
        {

        }

        private void listHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void CheckUpdates_Click(object? sender, EventArgs e)
        {
            await CheckForUpdatesAsync();
        }

        private void ChangeLanguage(Localization.Language language)
        {
            Localization.CurrentLanguage = language;

            // Reload the form with new language
            this.Controls.Clear();
            this.components?.Dispose();
            InitializeComponent();
            ApplyLocalization();

            LoadHistory();
            LoadFolderSizes();
        }

        private void ApplyLocalization()
        {
            lblLastRun.Text = Localization.Get("Stats.LastCleanup");
            lblTotalFreed.Text = Localization.Get("Stats.TotalFreed");
            colHistoryDate.Text = Localization.Get("Column.DateTime");
            colHistoryFreed.Text = Localization.Get("Column.Freed");
            colSizesFolder.Text = Localization.Get("Column.Location");
            colSizesSize.Text = Localization.Get("Column.Size");
            btnRefresh.Text = Localization.Get("Button.Scan");
            btnClearHistory.Text = Localization.Get("Button.ClearHistory");
            button1.Text = Localization.Get("Button.Cleanup");
            lblSubheader.Text = Localization.Get("Header.Subtitle");
            lblHistoryTitle.Text = Localization.Get("Section.History");
            lblSizesTitle.Text = Localization.Get("Section.Folders");
            nástrojeMenuItem.Text = Localization.Get("Menu.Tools");
            checkUpdatesMenuItem.Text = Localization.Get("Menu.CheckUpdates");
            Text = Localization.Get("Window.Title");
        }

        private void czechMenuItem_Click(object? sender, EventArgs e)
        {
            ChangeLanguage(Localization.Language.Czech);
        }

        private void englishMenuItem_Click(object? sender, EventArgs e)
        {
            ChangeLanguage(Localization.Language.English);
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Maiden-App");

                    var response = await client.GetAsync("https://api.github.com/repos/Jaraslama/Maid/releases");

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Localization.Get("Updates.CheckError"), Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        var releases = doc.RootElement.EnumerateArray();

                        if (!releases.Any())
                        {
                            MessageBox.Show(Localization.Get("Updates.NoReleases"), Localization.Get("Dialog.Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var latestRelease = releases.First();
                        string latestVersion = latestRelease.GetProperty("tag_name").GetString() ?? "Unknown";
                        string downloadUrl = latestRelease.GetProperty("html_url").GetString() ?? "";

                        string currentVersion = "v1.3.0"; // Change this to your actual version

                        if (latestVersion != currentVersion)
                        {
                            DialogResult result = MessageBox.Show(
                                $"{Localization.Get("Updates.NewVersion")} {latestVersion}{Localization.Get("Updates.DownloadQuestion")}",
                                Localization.Get("Updates.UpdateAvailable"),
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information
                            );

                            if (result == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = downloadUrl,
                                    UseShellExecute = true
                                });
                            }
                        }
                        else
                        {
                            MessageBox.Show(Localization.Get("Updates.UpToDate"), Localization.Get("Updates.Current"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Localization.Get("Dialog.Error")}: {ex.Message}", Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearHistory_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                Localization.Get("Dialog.ConfirmClearHistory"),
                Localization.Get("Dialog.ConfirmTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (File.Exists(LogFile))
                    {
                        File.Delete(LogFile);
                    }

                    listHistory.Items.Clear();
                    lblLastRun.Text = Localization.Get("Stats.LastCleanup");
                    lblTotalFreed.Text = Localization.Get("Stats.TotalFreed");

                    MessageBox.Show(Localization.Get("Dialog.ClearSuccess"), Localization.Get("Dialog.Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Localization.Get("Dialog.ClearError")} {ex.Message}", Localization.Get("Dialog.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    class CleanupEntry
    {
        public DateTime Timestamp { get; set; }
        public long BytesFreed { get; set; }
    }
}