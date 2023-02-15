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
    public partial class ShortcutPage : UserControl, ILoadablePage, INotifyPropertyChanged {

        private Shortcut _shortcut = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string _pageTitle = "";
        public string PageTitle {
            get => _pageTitle;
            set {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }

        private bool _isNewShortcut = true;
        public bool IsNewShortcut {
            get => _isNewShortcut;
            set {
                _isNewShortcut = value;
                PageTitle = _isNewShortcut ? "new shortcut" : "edit shortcut";
            }
        }
        public Shortcut Shortcut {
            get => _shortcut;
            set {
                _shortcut = value;
                OnPropertyChanged(nameof(Shortcut));
            }
        }

        public ShortcutPage() {
            InitializeComponent();
            this.DataContext = this;
        }

        public void LoadPage() {
            Shortcut = new();
            AddNewContainer();
            IsNewShortcut = true;
        }

        public void LoadPage(params object[] parameters) {
            if (parameters.Length > 0 && parameters[0] is Shortcut shortcut) {
                Shortcut = shortcut;
                IsNewShortcut = false;
            }
            else
                LoadPage();

            tb_ShortcutName.Focus();
            tb_ShortcutName.CaretIndex = tb_ShortcutName.Text.Length;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {

            Shortcut? d = Globals.Shortcuts.Where(s => s.Id == Shortcut.Id).FirstOrDefault();
            if (d != default && !d.DeepEqual(Shortcut)) {
                var mb = new PopUps.MessageBox_YesNo(App.Current.MainWindow,
                        $"You have changed your shortcut!\nAre you sure you don't want to save your changes?",
                        "Shortcut changed");

                mb.ShowDialog();

                // let user manually save if they want to store changes
                if (mb.Result != MessageBoxResult.Yes)
                    return;
            }

            PageManager.Instance.SwitchToPage("overview");
        }

        private void btnStore_Click(object sender, RoutedEventArgs e) {
            if (!Utils.ShortcutBuilder.ValidateShortcut(Shortcut, showWarningToUser:true))
                return;
            StoreShortcut();
            PageManager.Instance.SwitchToPage("overview");
        }

        private void StoreShortcut() {
            TidyUpShortcut();

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

        private void TidyUpShortcut() {
            for (int i = Shortcut.Containers.Count - 1; i >= 0; i--) {
                CommandContainer container = Shortcut.Containers[i];

                // drop empty commands
                for (int j = container.Commands.Count - 1; j >= 0; j--) {
                    Command command = container.Commands[j];
                    // only keep commands that are different to newly initialized ones
                    if (command.IsDefault())
                        container.Commands.Remove(command);
                }

                // only keep containers that are different to newly initialized ones
                if (container.IsDefault())
                    Shortcut.Containers.Remove(container);
                else {
                    // still require containers to have at least one command ...
                    if (container.Commands.Count == 0)
                        container.Commands.Add(new());
                }
            }
            // require to have at least one container per shortcut
            if (Shortcut.Containers.Count == 0)
                AddNewContainer();
        }

        private void btnAddCommand_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button)
                if (button.DataContext is CommandContainer container)
                    AddNewCommandToContainer(container);
        }

        private void btnRemoveCommand_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button) {
                if (button.DataContext is Command command) {
                    foreach (var container in Shortcut.Containers) {
                        var matchingCommands = container.Commands.Where(com => com.Id == command.Id);
                        if (matchingCommands.Any())
                            container.Commands.Remove(matchingCommands.First());

                        // it would not make any sense to have no commands in a specific container
                        if (container.Commands.Count == 0)
                            AddNewCommandToContainer(container);
                    }
                }
            }
        }

        private void btnAddContainer_Click(object sender, RoutedEventArgs e) => AddNewContainer();

        private void btnRemoveContainer_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button) {
                if (button.DataContext is CommandContainer container) {
                    Shortcut.Containers.Remove(container);

                    // it would not make any sense to have a command without containers
                    if (Shortcut.Containers.Count == 0)
                        AddNewContainer();
                }
            }
        }

        private void AddNewContainer() {
            Shortcut.Containers.Add(new CommandContainer() { Commands = new() { new() } });
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

        private void CommandDescriptionField_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (sender is TextBox tb) {
                    if (tb.DataContext is Command cmd) {
                        cmd.IsCommandFocused = true;
                    }
                }
            }
        }

        private void CommandExecutionField_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (sender is TextBox tb) {
                    if (tb.DataContext is Command cmd) {
                        CommandContainer? parentContainer = GetCommandParentContainer(cmd);
                        if (parentContainer != null && parentContainer.Commands.Contains(cmd)) {
                            // if command is the last in the container, we add a new command
                            if (parentContainer.Commands.Last().Equals(cmd)) {
                                if (!string.IsNullOrWhiteSpace(cmd.ExecutionString))
                                    AddNewCommandToContainer(parentContainer);
                            }
                            // otherwise we just switch the focus to the next command
                            else {
                                int idx = parentContainer.Commands.IndexOf(cmd);
                                Command nextCommand = parentContainer.Commands[idx + 1];

                                if (parentContainer.ShowDescriptionFields)
                                    nextCommand.IsDescriptionFocused = true;
                                else
                                    nextCommand.IsCommandFocused = true;                                
                            }                                
                        }
                    }
                }
            }
        }

        private CommandContainer? GetCommandParentContainer(Command command) => Shortcut.Containers
                            .Where(container => container.Commands.Any(c => c.Id == command.Id))
                            .FirstOrDefault();

        private void AddCommandToContainer(CommandContainer container, Command command) {
            container.Commands.Add(command);
            // provide proper focus on next text field
            if (container.ShowDescriptionFields)
                command.IsDescriptionFocused = true;
            else
                command.IsCommandFocused = true;
        }

        private void AddNewCommandToContainer(CommandContainer container) => AddCommandToContainer(container, new Command());
    }
}
