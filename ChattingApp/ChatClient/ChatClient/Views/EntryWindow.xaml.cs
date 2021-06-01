﻿using ChatClient.Utility;
using ChatClient.ViewModels;
using ChatLibrary;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.Views
{
    public partial class EntryWindow : Window
    {
        //private UserDetails userDetails;
        //MainWindowVM main = new();

        //private readonly Empty empty = new();
        public EntryWindow()
        {      
            InitializeComponent();
            //userDetails = new UserDetails();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Close();
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //_ = SendUserRequest();
            //_ = SendMessageRequest();

            //var users = GetUserListRequest();
            //ObservableCollection<string> userList = new();
            //foreach (User user in users.9Users)
            //{
            //    userList.Add(user.Name);
            //}
            //main.UserList = userList;

            //var messages = GetMessageListRequest();
            //ObservableCollection<string> messageList = new();
            //foreach (Message message in messages.Messages)
            //{
            //    messageList.Add(message.Text);
            //}
            //main.MessageList = messageList;

            //var rand = new Random();
            //userDetails.Id = rand.Next(0, 10);
            //userDetails.Name = UsernameText.Text;

            MainWindow mainWindow = new(UsernameText.Text);
            mainWindow.Show();

            Close();
        }

        //private async Task SendUserRequest()
        //{
        //    var rand = new System.Random();
        //    var client = new GrpcServiceProvider().GetMessengerClient();
        //    User usersv = new()
        //    {
        //        Name = UsernameText.Text,
        //        Id = rand.Next(0, 10)
        //    };

        //    await client.GetUserAsync(request: new GetUserRequest { User = usersv });
        //}

        //private async Task SendMessageRequest()
        //{
        //    var rand = new System.Random();
        //    var client = new GrpcServiceProvider().GetMessengerClient();
        //    Message messagesv = new()
        //    {
        //        Text = UsernameText.Text,
        //        User = new User() { Id = userDetails.Id, Name = userDetails.Name }
        //    };

        //    await client.SendMessageAsync(request: new SendRequest { Message = messagesv });
        //}

        //private SendUserListResponse GetUserListRequest()
        //{
        //    var client = new GrpcServiceProvider().GetMessengerClient();
        //    return client.SendUserList(request: empty);
        //}

        //private SendMessageListResponse GetMessageListRequest()
        //{
        //    var client = new GrpcServiceProvider().GetMessengerClient();
        //    return client.SendMessageList(request: empty);
        //}
    }
}