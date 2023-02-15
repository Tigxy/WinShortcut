using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace win_short_cut.ContentElements {
    /// <summary>
    /// Opens <see cref="Hyperlink.NavigateUri"/> in a default system browser.
    /// Taken from https://stackoverflow.com/a/27609749
    /// </summary>
    public class ExternalBrowserHyperlink : Hyperlink {
        public ExternalBrowserHyperlink() {
            RequestNavigate += OnRequestNavigate;
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
