using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using win_short_cut.DataClasses;

namespace win_short_cut.Pages
{
    /// <summary>
    /// Interaktionslogik für ShortcutsPage.xaml
    /// </summary>
    public partial class ShortcutsPage : Page, ILoadablePage
    {
        public ObservableCollection<Shortcut> Shortcuts { get; set; }

        public ShortcutsPage(Shortcuts shortcuts)
        {
            InitializeComponent();
            Shortcuts = new ObservableCollection<Shortcut>(shortcuts);
            this.DataContext = this;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut)
                    System.Diagnostics.Debug.WriteLine(shortcut.Name);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
                if (btn.DataContext is Shortcut shortcut)
                    PageManager.Instance.SwitchToPage("edit", shortcut);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            Shortcut newShortcut = new();
            Shortcuts.Add(newShortcut);
            PageManager.Instance.SwitchToPage("edit", newShortcut);
        }

        public void LoadPage()
        {
            throw new NotImplementedException();
        }

        public void LoadPage(params object[] parameters)
        {
            //throw new NotImplementedException();
        }
    }
}
