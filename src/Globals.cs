using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using win_short_cut.DataClasses;

namespace win_short_cut {
    public class Globals {

        public static readonly string ApplicationDirDeprecated = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "win-shortcut");
        public static readonly string ApplicationDir = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "WinShortcut");
        public static readonly string ShortcutBatPath = System.IO.Path.Combine(ApplicationDir, "bat");
        public static readonly string ShortcutPath = System.IO.Path.Combine(ApplicationDir, "shortcuts");

        public static readonly string ShortcutDataFile = System.IO.Path.Combine(ApplicationDir, "shortcuts.xml");
        public static readonly string SettingsFile = System.IO.Path.Combine(ApplicationDir, "settings.json");

        public static ObservableCollection<Shortcut> Shortcuts { get; set; } = new ObservableCollection<Shortcut>();
    }
}
