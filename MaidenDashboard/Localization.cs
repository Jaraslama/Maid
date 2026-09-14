using System;
using System.Globalization;
using System.IO;

namespace MaidenDashboard
{
    public static class Localization
    {
        public enum Language { Czech, English }
        public static Language CurrentLanguage { get; set; } = Language.Czech;

        public static string Get(string key)
        {
            return CurrentLanguage == Language.Czech ? GetCzech(key) : GetEnglish(key);
        }

        private static string GetCzech(string key) => key switch
        {
            // Header
            "Header.Title" => "Maiden",
            "Header.Subtitle" => "Nástroj pro čištění | optimalizaci systému",

            // Stats
            "Stats.LastCleanup" => "⏱️  Poslední čištění: -",
            "Stats.TotalFreed" => "💾 Celkem uvolněno: -",

            // Sections
            "Section.History" => "📋 Historie čištění",
            "Section.Folders" => "📁 Složky k čištění",

            // Column Headers
            "Column.DateTime" => "Datum/Čas",
            "Column.Freed" => "Uvolněno",
            "Column.Location" => "Umístění",
            "Column.Size" => "Aktuální velikost",

            // Buttons
            "Button.Scan" => "🔄 Kontrola",
            "Button.ClearHistory" => "❌ Vymazat Histori",
            "Button.Cleanup" => "🧹 Čištění",

            // Menu
            "Menu.Language" => "🌐 Jazyk",
            "Menu.Czech" => "Čeština",
            "Menu.English" => "Angličtina",
            "Menu.Tools" => "Nástroje",
            "Menu.CheckUpdates" => "🔍 Kontrola aktualizací",

            // Folder Names
            "Folder.UserTemp" => "Dočasné soubory uživatele",
            "Folder.WinLogs" => "Protokoly Windows",
            "Folder.WinTemp" => "Dočasné soubory systému",
            "Folder.UpdateCache" => "Mezipaměť aktualizací",

            // Status Messages
            "Status.NoScanYet" => "(ještě nebyla spuštěna kontrola)",
            "Status.ErrorReading" => "(chyba při čtení dat)",
            "Status.LastRunError" => "Poslední čištění: (chyba při čtení)",

            // Dialog Messages
            "Dialog.ConfirmClearHistory" => "Opravdu chcete vymazat historii čištění?\n\nTuto akci nelze vrátit.",
            "Dialog.ConfirmTitle" => "Potvrzení",
            "Dialog.ClearSuccess" => "Historie jste úspěšně vymazali.",
            "Dialog.ClearError" => "Chyba při mazání historie:",
            "Dialog.Success" => "Úspěch",
            "Dialog.Info" => "Info",
            "Dialog.Error" => "Chyba",

            // Updates
            "Updates.CheckError" => "Nelze se připojit ke GitHubu. Zkuste to později.",
            "Updates.NoReleases" => "Žádné verze nejsou k dispozici.",
            "Updates.NewVersion" => "Nová verze dostupná:",
            "Updates.DownloadQuestion" => "\n\nChcete ji stáhnout?",
            "Updates.UpdateAvailable" => "Aktualizace dostupná",
            "Updates.UpToDate" => "Máte nejnovější verzi!",
            "Updates.Current" => "Aktuální",

            // Window Title
            "Window.Title" => "Maiden - Čištění systému",

            // Default
            _ => key
        };

        private static string GetEnglish(string key) => key switch
        {
            // Header
            "Header.Title" => "Maiden",
            "Header.Subtitle" => "System Cleanup | Optimization Utility",

            // Stats
            "Stats.LastCleanup" => "⏱️  Last cleanup: -",
            "Stats.TotalFreed" => "💾 Total freed: -",

            // Sections
            "Section.History" => "📋 Cleanup History",
            "Section.Folders" => "📁 Folders to Clean",

            // Column Headers
            "Column.DateTime" => "Date/Time",
            "Column.Freed" => "Freed",
            "Column.Location" => "Location",
            "Column.Size" => "Current Size",

            // Buttons
            "Button.Scan" => "🔄 Scan",
            "Button.ClearHistory" => "❌ Clear",
            "Button.Cleanup" => "🧹 Cleanup",

            // Menu
            "Menu.Language" => "🌐 Language",
            "Menu.Czech" => "Čeština (Czech)",
            "Menu.English" => "English",
            "Menu.Tools" => "Tools",
            "Menu.CheckUpdates" => "🔍 Check for Updates",

            // Folder Names
            "Folder.UserTemp" => "User Temporary Files",
            "Folder.WinLogs" => "Windows Logs",
            "Folder.WinTemp" => "Windows Temporary Files",
            "Folder.UpdateCache" => "Windows Update Cache",

            // Status Messages
            "Status.NoScanYet" => "(no scan yet)",
            "Status.ErrorReading" => "(error reading data)",
            "Status.LastRunError" => "Last cleanup: (error reading)",

            // Dialog Messages
            "Dialog.ConfirmClearHistory" => "Are you sure you want to delete the cleanup history?\n\nThis action cannot be undone.",
            "Dialog.ConfirmTitle" => "Confirmation",
            "Dialog.ClearSuccess" => "History cleared successfully.",
            "Dialog.ClearError" => "Error clearing history:",
            "Dialog.Success" => "Success",
            "Dialog.Info" => "Info",
            "Dialog.Error" => "Error",

            // Updates
            "Updates.CheckError" => "Cannot connect to GitHub. Try again later.",
            "Updates.NoReleases" => "No releases available.",
            "Updates.NewVersion" => "New version available:",
            "Updates.DownloadQuestion" => "\n\nDo you want to download it?",
            "Updates.UpdateAvailable" => "Update Available",
            "Updates.UpToDate" => "You have the latest version!",
            "Updates.Current" => "Current",

            // Window Title
            "Window.Title" => "Maiden - System Cleanup",

            // Default
            _ => key
        };
    }
}
