using IWshRuntimeLibrary;

namespace win_short_cut.Utils {
    internal static class Sys {
        // based on http://www.jose.it-berater.org/smfforum/index.php?topic=2158.0
        public enum ShortcutWindowStyle {
            // Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position.
            Normal = 1,
            // Activates the window and displays it as a maximized window. 
            Maximized = 3,
            // Minimizes the window and activates the next top-level window.
            Minimized = 7
        }
        
        public static void CreateShortcut(string sourcePath, string targetPath) => CreateShortcut(sourcePath, targetPath, ShortcutWindowStyle.Normal, "");
        
        public static void CreateShortcut(string sourcePath, string targetPath, ShortcutWindowStyle windowStyle = ShortcutWindowStyle.Normal, string hotkey = "",
            string workingDirectory = "") {
            // see https://stackoverflow.com/a/18024472 and https://stackoverflow.com/a/18024472
            WshShell wsh = new();
            if (wsh.CreateShortcut(targetPath) is IWshShortcut shortcut) {
                //shortcut.Arguments = "";
                shortcut.TargetPath = sourcePath;
                shortcut.WindowStyle = (int)windowStyle;
                //shortcut.Description = "";
                shortcut.WorkingDirectory = workingDirectory;
                //shortcut.IconLocation = "";
                shortcut.Hotkey = hotkey;
                shortcut.Save();
            }
        }
    }
}
