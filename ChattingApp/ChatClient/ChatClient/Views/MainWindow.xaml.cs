using ChatClient.Utility;
using ChatLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string username,
            List<string> users,
            List<string> messages,
            UserDetails userDetails)
        {
            InitializeComponent();

            UsernameBlock.Text = username;
            UserList.ItemsSource = users;
            MessageList.ItemsSource = messages;
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
            Message chatsv = new Message()
            {
                Text = UserText.Text,
                User = new User()
                {
                    Id = rand.Next(0, 10),
                    Name = UsernameBlock.Text
                },
            };
            var reply = await client.SendMessageAsync(request: new SendRequest { Message = chatsv });
        }
    }
}