using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using win_short_cut.DataClasses;
using win_short_cut.Pages;
using win_short_cut.Utils;

namespace win_short_cut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DefaultShortcutPath = "";
        public static readonly string ApplicationDir = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "win-shortcut");
        public static readonly string ShortcutBinPath = System.IO.Path.Combine(ApplicationDir, "bin");

        public static readonly string ShortcutDataFile = System.IO.Path.Combine(ApplicationDir, "shortcuts.xaml");
        public static readonly string SettingsFile = System.IO.Path.Combine(ApplicationDir, "settings.json");

        public Shortcuts Shortcuts = new();

        public MainWindow()
        {
            InitializeComponent();

            // ensure that necessary directories exist
            System.IO.Directory.CreateDirectory(ApplicationDir);
            System.IO.Directory.CreateDirectory(ShortcutBinPath);

            // make shortcut files visible for command prompt
            if (!Utils.Environment.IsInPath(ShortcutBinPath))
                Utils.Environment.AddToPath(ShortcutBinPath);

            LoadSettings();
            LoadShortcuts();
            GenerateShortcutFiles();

            PageManager.Instance.RegisterPage("overview", new ShortcutsPage(Shortcuts));
            PageManager.Instance.RegisterPage("edit", new ShortcutPage());

            PageManager.Instance.SwitchToPage("overview");
        }

        private void GenerateShortcutFiles()
        {
            foreach (var shortcut in Shortcuts)
                ShortcutToFile(shortcut);
        }

        private void ShortcutToFile(Shortcut shortcut)
        {
            Utils.BatBuilder bb = new();

            bb.Comment(@"This file was automatically created by 'win-shortcut'.");
            bb.Comment(new string('=', 40));
            bb.Comment("Description: " + shortcut.Description);
            bb.Comment(new string('=', 40));

            for (int i = 0; i < shortcut.Containers.Length; i++)
            {
                var container = shortcut.Containers[i];
                string containerFile = System.IO.Path.Combine(ShortcutBinPath, $"{shortcut.Name}__{i}.bat");

                string command = container.KeepOpenOnceDone 
                    ? $"start cmd.exe /k \"{containerFile}\"" 
                    : $"start cmd.exe /c \"{containerFile}\"";
                bb.Execute(command, true);

                ContainerToFile(container, containerFile);
            }

            bb.ToFile(System.IO.Path.Combine(ShortcutBinPath, $"{shortcut.Name}.bat"));
        }

        private void ContainerToFile(CommandContainer container, string path)
        {
            Utils.BatBuilder bb = new();

            bb.Comment(@"This file was automatically created by 'win-shortcut'.");
            bb.Comment(new string('=', 40));
            bb.Comment("Description: " + container.Description);
            bb.Comment(new string('=', 40));

            container.Commands.ForEach(command =>
            {
                if (!String.IsNullOrWhiteSpace(command.Description))
                {
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StoreSettings();
            StoreShortcuts();
        }

        private void LoadSettings()
        {
            (bool success, Settings? settings) = Utils.Serialization.LoadJson<Settings>(SettingsFile);

            if (success)
            {
                this.Width = settings!.WindowWidth;
                this.Height = settings!.WindowHeight;

                this.Top = settings!.WindowTop;
                this.Left = settings!.WindowLeft;
            }
            else
            {
                // set default startup location in case no previous settings are known
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        private void StoreSettings()
        {
            Utils.Serialization.StoreJson<Settings>(new()
            {
                WindowHeight = this.Height,
                WindowWidth = this.Width,
                WindowLeft = this.Left,
                WindowTop = this.Top
            }, SettingsFile);
        }

        private void StoreShortcuts() => Utils.Serialization.Store<Shortcuts>(new Shortcuts(this.Shortcuts.ToArray()), ShortcutDataFile);
        private void LoadShortcuts()
        {
            (bool success, Shortcuts? data) = Utils.Serialization.Load<Shortcuts>(ShortcutDataFile);
            if (success)
                data!.ForEach(x => this.Shortcuts.Add(x));
        }

        //private void btn_settings_Click(object sender, RoutedEventArgs e)
        //{
        //    fr_main.Content = new SettingsPage();
        //}

        //private void btn_shortcuts_Click(object sender, RoutedEventArgs e)
        //{
        //    fr_main.Content = new ShortcutPage();
        //}


        private Shortcut CreateDummyShortcut()
        {
            return new Shortcut()
            {
                Name = "test",
                Description = "Some test shortcut",
                Containers = new[] {
                    new CommandContainer() {
                        KeepOpenOnceDone = true,
                        Description = "one container",
                        Commands = new [] {
                            new Command() { ExecutionString = "explorer.exe", PrintCommand = true}
                        }
                    },
                    new CommandContainer() {
                        KeepOpenOnceDone = true,
                        Description = "another container",
                        Commands = new [] {
                            new Command() { ExecutionString = "explorer.exe", PrintCommand = true},
                            new Command() { ExecutionString = "explorer.exe", PrintCommand = true}
                        }
                    }
                }
            };
        }
    }
}
