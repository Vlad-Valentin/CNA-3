using ChatClient.Grpc;
using ChatClient.ViewModels;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.Views
{
    public partial class MainWindow : Window
    {
        private static List<string> listOfMessages = new();
        private static GrpcChannel RpcChannel = GrpcChannel.ForAddress("https://localhost:5001");
        private static MessengerService.MessengerServiceClient ChatClient = new MessengerService.MessengerServiceClient(RpcChannel);

        private ChatClientRpc client;

        public MainWindow(string username)
        {
            //_ = Join(username);
            DataContext = new MainWindowVM();

            InitializeComponent();

            UsernameBlock.Text = username;
            client = new();
            client.RecievedMessages += OnMessageRecieved;
            client.RecievedUsers += OnUserRecieved;
            SendUserToServer(username);
        }

        public void SendUserToServer(string username)
        {
            client.SendUser(username);
            client.RefreshUsers();
        }

        private void SetOnline()
        {
            //To-DO
        }

        private void SetOffline()
        {
            //To-DO
        }

        private void OnMessageRecieved(object sender, RecievedMessagesEventArgs args)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var msg in args.Messages)
                    {
                        MainWindowVM.MessageList.Add(msg.ToString());
                        
                        //FormatText(msg.ToString());

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

        private void OnUserRecieved(object sender,RecievedUsersEventArgs args)
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
            catch(System.Threading.Tasks.TaskCanceledException taskException)
            {
                Console.WriteLine(taskException.Message);
            }
        }

        //private void FormatText(string text)
        //{
        //    string s = text;
        //    var parts = s.Split(new[] { " _", "_ " }, StringSplitOptions.None);
        //    bool isbold = false;

        //    foreach (var part in parts)
        //    {
        //        if (isbold)
        //            MessageBox.ItemContainerStyle.Setters.Add(new Setter()
        //            {
        //                Property = FontStyleProperty,
        //                Value = FontStyles.Italic
        //            });
        //        else
        //            MessageBox.ItemContainerStyle.Setters.Add(new Setter()
        //            {
        //                Property = FontStyleProperty,
        //                Value = FontStyles.Normal
        //            });

        //        isbold = !isbold; // toggle between bold and not bold
        //    }
        //}

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

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Close();
            }
        }

        /* private async Task Join(string username)
         {
          
             recieveMessagesThread.Start();
             while (recieveMessagesThread.ThreadState != ThreadState.Stopped)
             {
                 foreach (var message in listOfMessages)
                 {
               
                 }
             }
             await ChatClient.JoinAsync(new Message { User = new User { Id = 0, Name = username }, Text = " is here" });
         }

         private void Send_Click(object sender, RoutedEventArgs e)
         {
             _ = SendMessage();
         }

         private async Task SendMessage()
         {
             //_ = CallGrpcService();
             _ = Join("");
             string msg;

             bool isConnected = true;
             //while (isConnected)
             //{
                 msg = UserText.Text;
                 if (msg.Equals("EXIT"))
                 {
                     isConnected = false;
                     await ChatClient.LeaveAsync(new Message { User = new User { Id = 0, Name = UsernameBlock.Text }, Text = msg });
                 }
                 else
                 {
                     await ChatClient.ClientToServerAsync(new Message { User = new User { Id = 0, Name = UsernameBlock.Text }, Text = msg });
                 }
             MainWindowVM.MessageList.Add(msg);
             //}
         }

         //private async Task CallGrpcService()
         //{
         //    /* var client = new GrpcServiceProvider().ge;
         //     var reply = await client.SayHelloAsync(
         //                       new HelloRequest { Name = "WPF client" });

         //     this.UserText.Text = reply.Message;*/
        //    //var rand = new Random();
        //    //var client = new GrpcServiceProvider().GetMessengerClient();
        //    //Message chatsv = new Message()
        //    //{
        //    //    Text = UserText.Text,
        //    //    User = new User()
        //    //    {
        //    //        Id = rand.Next(0, 10),
        //    //        Name = UsernameBlock.Text
        //    //    },
        //    //};
        //    //var reply = await client.SendMessageAsync(request: new SendRequest { Message = chatsv });
        //}

        /* private async void ServerToClient()
         {
             await Task.Run(async () =>
             {
                 var dataStream = ChatClient.ServerToClient(new Empty());

                 try
                 {
                     await foreach (var message in dataStream.ResponseStream.ReadAllAsync())
                     {
                         listOfMessages.Add(message.Text);
                     }
                 }
                 catch (RpcException e) when (e.StatusCode == StatusCode.Cancelled)
                 {
                     Console.WriteLine("Stream cancelled by client.");
                 }
             });
         }*/
    }
}