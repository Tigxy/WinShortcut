using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using win_short_cut.DataClasses;
using win_short_cut.Utils;

namespace win_short_cut.Pages {
    /// <summary>
    /// Interaktionslogik für InfoPage.xaml
    /// </summary>
    public partial class InfoPage : UserControl, ILoadablePage {

        public string Version { get; } = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "unknown";

        public InfoPage() {
            InitializeComponent();
            this.DataContext = this;
        }

        public void LoadPage() {

        }

        public void LoadPage(params object[] parameters) {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            PageManager.Instance.SwitchToPage("overview");
        }
    }
}
