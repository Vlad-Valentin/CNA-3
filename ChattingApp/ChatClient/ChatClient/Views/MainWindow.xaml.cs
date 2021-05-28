using ChatClient.Utility;
using ChatClient.ViewModels;
using ChatLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string username,
            ObservableCollection<string> users,
            ObservableCollection<string> messages,
            UserDetails userDetails)
        {
           DataContext = new MainWindowVM();

            InitializeComponent();

            UsernameBlock.Text = username;
            UserBox.ItemsSource = users;
            MessageBox.ItemsSource = messages;
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