using System.Windows;


namespace win_short_cut.PopUps {
    /// <summary>
    /// Interaktionslogik für MessageBox_YesNo.xaml
    /// </summary>
    public partial class MessageBox_YesNo : Window {
     
        public string Message { get; private set; }
        public MessageBoxResult Result = MessageBoxResult.None;

        public MessageBox_YesNo(string message, string title) {
            InitializeComponent();
            Message = message;
            Title = title;
            this.DataContext = this;
        }

        public MessageBox_YesNo(string message) : this(message, App.Current.MainWindow.Title){

        }

        public MessageBox_YesNo(Window parent, string message, string title) : this(message, title) {
            this.Owner = parent;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Icon = parent.Icon;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e) {
            Result = MessageBoxResult.No;
            this.Close();
        }
    }
}
