using System.Windows;


namespace win_short_cut.PopUps {
    /// <summary>
    /// Interaktionslogik für MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window {
     
        public string Message { get; private set; }

        public MessageBox(string message, string title) {
            InitializeComponent();
            Message = message;
            Title = title;
            this.DataContext = this;
        }

        public MessageBox(string message) : this(message, App.Current.MainWindow.Title){

        }

        public MessageBox(Window parent, string message, string title) : this(message, title){
            this.Owner = parent;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Icon = parent.Icon;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
