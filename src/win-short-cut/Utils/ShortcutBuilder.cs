using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using win_short_cut.DataClasses;

namespace win_short_cut.Utils {
    public static class ShortcutBuilder {
        public static void GenerateShortcutFiles() {
            foreach (var shortcut in Globals.Shortcuts)
                ShortcutToFile(shortcut);
        }

        public static void ShortcutToFile(Shortcut shortcut) {
            Utils.BatBuilder bb = new();

            bb.Comment(@"This file was automatically created by 'win-shortcut'.");
            bb.Comment(new string('=', 40));
            bb.Comment("Description: " + shortcut.Description);
            bb.Comment(new string('=', 40));

            if (shortcut.Containers != null) {
                for (int i = 0; i < shortcut.Containers.Count; i++) {
                    var container = shortcut.Containers[i];
                    string containerFile = System.IO.Path.Combine(Globals.ShortcutBinPath, $"{shortcut.Name}__{i}.bat");

                    string command = container.KeepOpenOnceDone
                        ? $"start cmd.exe /k \"{containerFile}\""
                        : $"start cmd.exe /c \"{containerFile}\"";
                    bb.Execute(command, true);

                    ContainerToFile(container, containerFile);
                }
            }

            bb.ToFile(System.IO.Path.Combine(Globals.ShortcutBinPath, $"{shortcut.Name}.bat"));
        }

        public static void DeleteShortcut(Shortcut shortcut) {
            string filePath = System.IO.Path.Join(Globals.ShortcutBinPath, $"{shortcut.Name}.bat");
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        private static void ContainerToFile(CommandContainer container, string path) {
            Utils.BatBuilder bb = new();

            bb.Comment(@"This file was automatically created by 'win-shortcut'.");
            bb.Comment(new string('=', 40));
            bb.Comment("Description: " + container.Description);
            bb.Comment(new string('=', 40));

            container.Commands.ForEach(command => {
                if (!String.IsNullOrWhiteSpace(command.Description)) {
                    // No need to set a comment if we already echo it
                    if (command.PrintDescription)
                        bb.Echo(command.Description);
                    else
                        bb.Comment(command.Description);
                }

                bb.Execute(command.ExecutionString, silent: !command.PrintCommand);
            });

            bb.ToFile(path);
        }
    }
}
