using System.Windows;
using win_short_cut.DataClasses;
using win_short_cut.Pages;

namespace win_short_cut {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // for handling old versions
            if (System.IO.Directory.Exists(Globals.ApplicationDirDeprecated)) {
                if (System.IO.Directory.Exists(Globals.ApplicationDir))
                    // this statement might never be reached, but let's still ensure that everything is cleaned up
                    System.IO.Directory.Delete(Globals.ApplicationDirDeprecated, true);
                else
                    System.IO.Directory.Move(Globals.ApplicationDirDeprecated, Globals.ApplicationDir);
            }

            // ensure that necessary directories exist
            System.IO.Directory.CreateDirectory(Globals.ApplicationDir);
            System.IO.Directory.CreateDirectory(Globals.ShortcutBatPath);
            System.IO.Directory.CreateDirectory(Globals.ShortcutPath);

            // remove depricated shortcut path from the environment paths
            string deprecatedShortcutPath = System.IO.Path.Combine(Globals.ApplicationDir, "bin");
            if (Utils.Environment.IsInPath(deprecatedShortcutPath))
                Utils.Environment.RemoveFromPath(deprecatedShortcutPath);

            // make shortcut files visible in environment
            if (!Utils.Environment.IsInPath(Globals.ShortcutPath))
                Utils.Environment.AddToPath(Globals.ShortcutPath);

            LoadSettings();
            LoadShortcuts();

            PageManager.Instance.RegisterPage("overview", new ShortcutsPage());
            PageManager.Instance.RegisterPage("edit", new ShortcutPage());
            PageManager.Instance.RegisterPage("info", new InfoPage());

            PageManager.Instance.SwitchToPage("overview");
        }

        public void BringToForeground() {
            if (this.WindowState == WindowState.Minimized || this.Visibility == Visibility.Hidden) {
                this.Show();
                this.WindowState = WindowState.Normal;
            }

            // According to some sources these steps gurantee that an app will be brought to foreground.
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StoreSettings();
            StoreShortcuts();
        }

        private void LoadSettings()
        {
            (bool success, Settings? settings) = Utils.Serialization.LoadJson<Settings>(Globals.SettingsFile);

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
            Utils.Serialization.StoreJson<Settings>(new Settings()
            {
                WindowHeight = this.Height,
                WindowWidth = this.Width,
                WindowLeft = this.Left,
                WindowTop = this.Top
            }, Globals.SettingsFile);
        }

        private void StoreShortcuts() => Utils.Serialization.Store<Shortcuts>(new Shortcuts(Globals.Shortcuts), Globals.ShortcutDataFile);
        private void LoadShortcuts()
        {
            (bool success, Shortcuts? data) = Utils.Serialization.Load<Shortcuts>(Globals.ShortcutDataFile);
            if (success)
                data!.ForEach(x => Globals.Shortcuts.Add(x));
        }
    }
}
