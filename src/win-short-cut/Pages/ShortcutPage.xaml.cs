using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using win_short_cut.DataClasses;
using win_short_cut.Utils;

namespace win_short_cut.Pages {
    /// <summary>
    /// Interaktionslogik für ShortcutPage.xaml
    /// </summary>
    public partial class ShortcutPage : Page, ILoadablePage, INotifyPropertyChanged {

        private Shortcut _shortcut = new Shortcut();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Shortcut Shortcut {
            get => _shortcut;
            set {
                _shortcut = value;
                OnPropertyChanged(nameof(Shortcut));
                this.DataContext = Shortcut;
            }
        }

        public ShortcutPage() {
            InitializeComponent();
            this.DataContext = Shortcut;
        }

        public void LoadPage() {
            Shortcut = new();
        }

        public void LoadPage(params object[] parameters) {
            if (parameters.Length > 0 && parameters[0] is Shortcut shortcut)
                Shortcut = shortcut;
            else
                LoadPage();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            PageManager.Instance.SwitchToPage("overview");
        }

        private void btnStore_Click(object sender, RoutedEventArgs e) {
            if (!CheckValidity())
                return;
            StoreShortcut();
            PageManager.Instance.SwitchToPage("overview");
        }

        private bool CheckValidity() {
            if (String.IsNullOrWhiteSpace(Shortcut.Name)) {
                var mb = new PopUps.MessageBox(App.Current.MainWindow, $"Shortcut name must not be left empty!", "Save shortcut");
                mb.ShowDialog();
                return false;
            }

            if (Globals.Shortcuts.Where(x => x.Name == Shortcut.Name && x.Id != Shortcut.Id).Any()) {
                var mb = new PopUps.MessageBox(App.Current.MainWindow, $"Shortcut with identical name already exists!", "Save shortcut");
                mb.ShowDialog();
                return false;
            }

            return true;
        }

        private void StoreShortcut() {
            bool foundMatch = false;
            for (int i = 0; i < Globals.Shortcuts.Count; i++) {
                // compare ids and take over all changes
                if (Globals.Shortcuts[i].Id == Shortcut.Id) {
                    // on rename we need to remove the old shortcut file, otherwise it will get lost
                    Utils.ShortcutBuilder.DeleteShortcut(Globals.Shortcuts[i]);
                    Globals.Shortcuts[i] = Shortcut;
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
                // no match means that passed shortcut is a new one
                Globals.Shortcuts.Add(Shortcut);

            Utils.ShortcutBuilder.ShortcutToFile(Shortcut);
        }
        private void btnAddCommand_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button)
                if (button.DataContext is CommandContainer container)
                    container.Commands.Add(new Command());
        }

        private void btnRemoveCommand_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button) {
                // totally hacky solution via Id..
                // TODO: solve this by going up the visual tree and finding FrameworkElement where DataContext matches CommandContainer
                if (button.DataContext is Command command) {
                    foreach (var container in Shortcut.Containers) {
                        var matchingCommands = container.Commands.Where(com => com.Id == command.Id);
                        if (matchingCommands.Any())
                            container.Commands.Remove(matchingCommands.First());
                    }
                }
            }
        }

        private void btnAddContainer_Click(object sender, RoutedEventArgs e) {
            Shortcut.Containers.Add(new CommandContainer());
        }

        private void btnRemoveContainer_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button) {
                if (button.DataContext is CommandContainer container) {
                    Shortcut.Containers.Remove(container);
                }
            }
        }

        private void lv_Commands_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {
            if (!e.Handled) {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                var parent = ((Control)sender).Parent as UIElement;
                parent?.RaiseEvent(eventArg);
            }
        }
    }
}
