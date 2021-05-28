using ChatLibrary;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string username)
        {
            InitializeComponent();

            UsernameBlock.Text = username;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            _ = CallGrpcService();
        }

        private async Task CallGrpcService()
        {
            /* var client = new GrpcServiceProvider().ge;
             var reply = await client.SayHelloAsync(
                               new HelloRequest { Name = "WPF client" });

             this.UserText.Text = reply.Message;*/
            var rand = new Random();
            var client = new GrpcServiceProvider().GetMessengerClient();
            ChatService.Message chatsv = new ChatService.Message()
            {
                Text = UserText.Text,
                User = new ChatService.User()
                {
                    Id = rand.Next(0, 10),
                    Name = UsernameBlock.Text
                },
            };
            var reply = await client.SendMessageAsync(request: new ChatService.SendRequest { Message = chatsv });
        }
    }
}