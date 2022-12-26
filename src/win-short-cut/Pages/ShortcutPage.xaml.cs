using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using win_short_cut.DataClasses;

namespace win_short_cut.Pages {
    /// <summary>
    /// Interaktionslogik für ShortcutPage.xaml
    /// </summary>
    public partial class ShortcutPage : Page, ILoadablePage, INotifyPropertyChanged {

        private Shortcut? _shortcut = null;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Shortcut? Shortcut
        {
            get => _shortcut;
            set
            {
                _shortcut = value;
                OnPropertyChanged(nameof(Shortcut));
                this.DataContext = Shortcut;
            }
        }

        public ShortcutPage() {
            InitializeComponent();
            this.DataContext = Shortcut;
        }

        public void LoadPage()
        {
            throw new NotImplementedException();
        }

        public void LoadPage(params object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is Shortcut shortcut)
                Shortcut = shortcut;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageManager.Instance.SwitchToPage("overview");
        }

        private void btnStore_Click(object sender, RoutedEventArgs e)
        {
            if (Shortcut != null)
                PageManager.Instance.SwitchToPage("overview", Shortcut);
            else
                PageManager.Instance.SwitchToPage("overview");
        }
    }
}
