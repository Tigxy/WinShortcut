using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace win_short_cut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DefaultShortcutPath = "";
        public static readonly string ApplicationDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "win-shortcut");
        public static readonly string ShortcutPath = System.IO.Path.Combine(ApplicationDir, "bin");

        public Settings settings = new Settings {
            WindowHeight = 500,
            WindowWidth = 700,
            WindowTop = 500,
            WindowLeft = 1000,
        };

        public MainWindow()
        {
            InitializeComponent();

            fr_main.Content = new ShortcutPage();

            // ensure that necessary directories are created
            System.IO.Directory.CreateDirectory(ApplicationDir);
            System.IO.Directory.CreateDirectory(ShortcutPath);

            // make shortcut files visible for command prompt
            if (!Utils.Environment.IsInPath(ShortcutPath))
                Utils.Environment.AddToPath(ShortcutPath);
        }

        //private void btn_settings_Click(object sender, RoutedEventArgs e)
        //{
        //    fr_main.Content = new SettingsPage();
        //}

        //private void btn_shortcuts_Click(object sender, RoutedEventArgs e)
        //{
        //    fr_main.Content = new ShortcutPage();
        //}
    }
}
