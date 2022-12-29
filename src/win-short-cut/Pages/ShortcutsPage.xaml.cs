using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using win_short_cut.DataClasses;
using win_short_cut.Utils;

namespace win_short_cut.Pages {
    /// <summary>
    /// Interaktionslogik für ShortcutsPage.xaml
    /// </summary>
    public partial class ShortcutsPage : Page, ILoadablePage {

        public ShortcutsPage() {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut)
                    System.Diagnostics.Debug.WriteLine(shortcut.Name);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut)
                    PageManager.Instance.SwitchToPage("edit", shortcut.DeepClone());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e) {
            if (sender is Button btn) {
                if (btn.DataContext is Shortcut shortcut) {
                    var mb = new PopUps.MessageBox_YesNo(App.Current.MainWindow, 
                        $"Would you really like to delete the shortcut '{shortcut.Name}'?", 
                        "Delete shortcut");

                    mb.ShowDialog();

                    if (mb.Result == MessageBoxResult.Yes) {
                        Globals.Shortcuts.Remove(shortcut);

                        // also delete corresponding bat file
                        Utils.ShortcutBuilder.DeleteShortcut(shortcut);
                    }
                }
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e) {
            Shortcut newShortcut = new();
            PageManager.Instance.SwitchToPage("edit", newShortcut);
        }

        public void LoadPage() { }

        public void LoadPage(params object[] parameters) { }
    }
}
