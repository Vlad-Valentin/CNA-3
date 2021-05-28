using ChatLibrary;
using System;
using System.Threading.Tasks;
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
            _ = CallGrpcService();

            MainWindow mainWindow = new(UsernameText.Text);
            mainWindow.Show();

            Close();
        }

        private async Task CallGrpcService()
        {
            var rand = new Random();
            var client = new GrpcServiceProvider().GetMessengerClient();
            User usersv = new User()
            {
                Name = UsernameText.Text,
                Id = rand.Next(0, 10)

            };
            var reply = await client.GetUserAsync(request: new GetUserRequest { User = usersv });
        }
    }
}