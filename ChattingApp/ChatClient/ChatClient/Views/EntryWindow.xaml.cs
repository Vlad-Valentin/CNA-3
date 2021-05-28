using System.Windows;

namespace ChatClient.Views
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        public EntryWindow()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {         
            MainWindow mainWindow = new(UsernameText.Text);
            mainWindow.Show();

            Close();
        }
    }
}