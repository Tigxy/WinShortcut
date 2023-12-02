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
    public partial class ShortcutsPage : UserControl, ILoadablePage {

        public ShortcutsPage() {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e) {
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut)
                    Utils.ShortcutBuilder.ExecuteShortcut(shortcut);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut) {
                    if (shortcut.DeepCopy() is Shortcut shortcutCopy) {
                        PageManager.Instance.SwitchToPage("edit", shortcutCopy);
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("Error: could not duplicate shortcut for editing");
                        return;
                    }
                }
        }

        private void btnDuplicate_Click(object sender, RoutedEventArgs e) {
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut) {
                    var newShortcut = shortcut.DeepCopy(copyId: false);
                    if (newShortcut == null) {
                        System.Diagnostics.Debug.WriteLine("Error: could not duplicate shortcut");
                        return;
                    }

                    int i = 1;
                    string baseNameSuggestion = newShortcut.Name + "_copy";
                    string nameSuggestion = baseNameSuggestion;
                    while (Globals.Shortcuts.Where(s => s.Name == nameSuggestion).Any()) {
                        nameSuggestion = baseNameSuggestion + $"_{i}";
                        i += 1;
                    }

                    newShortcut.Name = nameSuggestion;
                    Globals.Shortcuts.Add(newShortcut);
                    Utils.ShortcutBuilder.ShortcutToFile(newShortcut);
                }
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
            PageManager.Instance.SwitchToPage("edit");
        }

        private void btnRefreshAll_Click(object sender, RoutedEventArgs e) {
            Utils.ShortcutBuilder.RecreateShortcutFiles();
        }

        private void btnShowInfo_Click(object sender, RoutedEventArgs e) {
            PageManager.Instance.SwitchToPage("info");
        }
        
        public void LoadPage() { }

        public void LoadPage(params object[] parameters) { }
    }
}
