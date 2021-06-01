using ChatClient.Grpc;
using ChatClient.Utility;
using ChatClient.ViewModels;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatClient.Views
{
    public partial class MainWindow : Window
    {
        private static readonly List<string> listOfMessages = new();
        private static readonly GrpcChannel RpcChannel = GrpcChannel.ForAddress("https://localhost:5001");
        private static readonly MessengerService.MessengerServiceClient ChatClient = new(RpcChannel);

        private readonly ChatClientRpc client;

        public MainWindow(string username)
        {
            DataContext = new MainWindowVM();

            InitializeComponent();

            UsernameBlock.Text = username;
            client = new();
            client.RecievedMessages += OnMessageRecieved;
            client.RecievedUsers += OnUserRecieved;
            client.LeftUsers += OnDcRecieved;
            SendUserToServer(username);
        }

        public void SendUserToServer(string username)
        {
            client.SendUser(username);
            client.RefreshUsers();
        }

        private void OnMessageRecieved(object sender, RecievedMessagesEventArgs args)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var msg in args.Messages)
                    {
                        TextBlock tb = TextFormatter.FormatText(msg);
                        ListOfMessages.Children.Add(tb);
                    }
                }));
            }
            catch (System.Threading.Tasks.TaskCanceledException taskException)
            {
                Console.WriteLine(taskException.Message);
                //continue by shutting down client
                //or restarting him
            }
        }

        private void OnDcRecieved(object sender, LeftUserEventArgs args)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var user in args.DisconnectedUsers)
                    {
                        if (MainWindowVM.UserList.Contains(user))
                        {
                            MainWindowVM.UserList.Remove(user);
                        }
                    }
                }));
            }
            catch (System.Threading.Tasks.TaskCanceledException taskException)
            {
                Console.WriteLine(taskException.Message);
            }
        }

        private void OnUserRecieved(object sender, RecievedUsersEventArgs args)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var user in args.Users)
                    {
                        MainWindowVM.UserList.Add(user.ToString());
                    }
                }));
            }
            catch (System.Threading.Tasks.TaskCanceledException taskException)
            {
                Console.WriteLine(taskException.Message);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                return;
            }
            try
            {
                client.SendMessage(UsernameBlock.Text.ToString(), UserText.Text.ToString());
                UserText.Text = string.Empty;
                client.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Disconnect_Click(object sender, CancelEventArgs e)
        {
            if (client == null)
            {
                return;
            }
            try
            {
                client.SendDisconnectedUser(UsernameBlock.Text.ToString());
                client.RefreshDisconnected();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Close();
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}