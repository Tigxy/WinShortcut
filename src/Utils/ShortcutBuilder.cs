using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using win_short_cut.DataClasses;

namespace win_short_cut.Utils {
    public static class ShortcutBuilder {
        public static string GetPrimaryBatPath(Shortcut shortcut) {
            return System.IO.Path.Join(Globals.ShortcutBatPath, $"{shortcut.Name}.bat");
        }

        public static string GetShortcutPath(Shortcut shortcut) {
            return System.IO.Path.Join(Globals.ShortcutPath, $"{shortcut.Name}.lnk");
        }

        public static void DeleteAllShortcutFiles() {
            System.IO.DirectoryInfo di = new(Globals.ShortcutBatPath);
            foreach (System.IO.FileInfo file in di.GetFiles()) {
                file.Delete();
            }
        }

        public static void RecreateShortcutFiles() {
            DeleteAllShortcutFiles();
            GenerateShortcutFiles();
        }

        public static void GenerateShortcutFiles() {
            foreach (var shortcut in Globals.Shortcuts)
                ShortcutToFile(shortcut);
        }

        public static bool ValidateShortcut(Shortcut shortcut, bool showWarningToUser=true) {
            if (String.IsNullOrWhiteSpace(shortcut.Name)) {
                if (showWarningToUser) {
                    var mb = new PopUps.MessageBox(App.Current.MainWindow, $"Please assign a name to your shortcut!", "Invalid shortcut");
                    mb.ShowDialog();
                }
                return false;
            }

            if (shortcut.Name.Contains("__")) {
                if (showWarningToUser) {
                    var mb = new PopUps.MessageBox(App.Current.MainWindow, $"Your shortcut name must not contain double underscore '_'!", "Invalid shortcut");
                    mb.ShowDialog();
                }
                return false;
            }

            if (Globals.Shortcuts.Where(x => x.Name == shortcut.Name && x.Id != shortcut.Id).Any()) {
                if (showWarningToUser) {
                    var mb = new PopUps.MessageBox(App.Current.MainWindow, $"Another shortcut with an identical name already exists!", "Invalid shortcut");
                    mb.ShowDialog();
                }
                return false;
            }

            return true;
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
                    string containerFile = System.IO.Path.Combine(Globals.ShortcutBatPath, $"{shortcut.Name}__{i}.bat");

                    char keepOpenFlag = container.KeepOpenOnceDone ? 'k' : 'c';
                    string command = $"cmd.exe /{keepOpenFlag} \"{containerFile}\"";

                    string newConsoleTitle = $"{shortcut.Name}";
                    if (shortcut.Containers.Count > 1)
                        newConsoleTitle += $"-{i}";

                    string startCommand = "start";
                    if (container.ExecuteMinimized)
                        startCommand += " /min";
                    command = $"{startCommand} \"{newConsoleTitle}\" {command}";

                    bb.Execute(command, true);

                    ContainerToFile(container, containerFile);
                }
            }

            // create shortcut bat file
            string batPath = GetPrimaryBatPath(shortcut);
            bb.ToFile(batPath);

            // create shortcut
            string shortcutPath = GetShortcutPath(shortcut);
            Sys.CreateShortcut(batPath, shortcutPath, Sys.ShortcutWindowStyle.Minimized);
        }

        public static void DeleteShortcut(Shortcut shortcut) {
            System.IO.DirectoryInfo di = new(Globals.ShortcutBatPath);
            foreach (System.IO.FileInfo file in di.GetFiles()) {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
             
                // delete all files that match current shortcut name
                Match m = Regex.Match(fileName, @$"{shortcut.Name}(__\d+)?");
                if (m.Success)
                    file.Delete();
            }

            string actShortcutPath = System.IO.Path.Join(Globals.ShortcutPath, shortcut.Name + ".lnk");
            if (System.IO.File.Exists(actShortcutPath))
                System.IO.File.Delete(actShortcutPath);
        }

        public static void ExecuteShortcut(Shortcut shortcut) {
            ProcessStartInfo startInfo = new() {
                FileName = GetShortcutPath(shortcut),
                // start in user directory to show a 'pretty' path in the command prompt
                WorkingDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
                UseShellExecute = true
            };
            Process process = new() { StartInfo = startInfo };
            process.Start();
        }

        private static void ContainerToFile(CommandContainer container, string path) {
            Utils.BatBuilder bb = new();

            bb.Comment(@"This file was automatically created by 'win-shortcut'.");
            bb.Comment(new string('=', 40));
            bb.Comment("Description: " + container.Description);
            bb.Comment(new string('=', 40));

            // execute all commands together, as this may help when later commands are getting lost due to e.g. 'conda activate'
            // for more info, see the questions to this StackOverflow question: https://stackoverflow.com/q/69068
            if (container.ConcatenateCommands)
                bb.Execute("(", silent:true);   // silence command, otherwise all inside brackets is repeated

            container.Commands.ForEach(command => {

                if (!String.IsNullOrWhiteSpace(command.Description)) {
                    // No need to set a comment if we already echo it
                    if (command.PrintDescription)
                        bb.Echo(command.Description, silent: !container.ConcatenateCommands);
                    else
                        bb.Comment(command.Description);
                }

                // simulate command output if requested
                if (container.ConcatenateCommands && command.PrintCommand) {
                    bb.Echo($"%cd%^>{command.ExecutionString} & :: simulate call", silent: false); // '^' to escape special '>' character, see https://stackoverflow.com/a/7308614
                }

                bb.Execute(command.ExecutionString, silent: !command.PrintCommand && !container.ConcatenateCommands);
            });

            if (container.ConcatenateCommands)
                bb.Execute(")");

            bb.ToFile(path);
        }
    }
}
