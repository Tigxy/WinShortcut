using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        public static void CreateShortcut(string sourcePath, string targetPath, ShortcutWindowStyle windowStyle = ShortcutWindowStyle.Normal, string hotkey = "") {
            // see https://stackoverflow.com/a/18024472 and https://stackoverflow.com/a/18024472
            WshShell wsh = new();
            if (wsh.CreateShortcut(targetPath) is IWshShortcut shortcut) {
                //shortcut.Arguments = "";
                shortcut.TargetPath = sourcePath;
                shortcut.WindowStyle = (int)windowStyle;
                //shortcut.Description = "";
                shortcut.WorkingDirectory = new global::System.IO.FileInfo(sourcePath).DirectoryName;
                //shortcut.IconLocation = "";
                shortcut.Hotkey = hotkey;
                shortcut.Save();
            }
        }
    }
}
