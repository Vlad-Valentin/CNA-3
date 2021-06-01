using ChatClient.Models;
using ChatLibrary;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient.Grpc
{
    public class RecievedMessagesEventArgs : EventArgs
    {
        public IEnumerable<ChatMessage> Messages;
    }

    public class RecievedUsersEventArgs : EventArgs
    {
        public IEnumerable<string> Users;
    }

    public class LeftUserEventArgs : EventArgs
    {
        public IEnumerable<string> DisconnectedUsers;
    }
    public class ChatClientRpc
    {
        public const int INTERVAL = 1000;

        public bool firstConnection = true;
        public bool firstUserConnection = true;
        public bool firstDcConnection = true;

        public delegate void MessagesRecievedHandler(object Sender, RecievedMessagesEventArgs args);
        public event MessagesRecievedHandler RecievedMessages;

        public delegate void UsersRecievedHandler(object sender, RecievedUsersEventArgs args);
        public event UsersRecievedHandler RecievedUsers;

        public delegate void LeftUserHandler(object sender, LeftUserEventArgs args);
        public event LeftUserHandler LeftUsers;

        private int updating = 0;
        private int userUpdating = 0;
        private int dcUpdating = 0;

        ChatClient.MessengerService.MessengerServiceClient client = new GrpcServiceProvider().GetMessengerClient();
        Timer timer;
        Timer userTimer;
        Timer dcTimer;

        IEnumerable<ChatMessage> messages = new List<ChatMessage>();
        IEnumerable<ChatMessage> oldMessages = new List<ChatMessage>();
        IEnumerable<string> users = new List<string>();
        IEnumerable<string> oldUsers = new List<string>();
        IEnumerable<string> disconnectedUsers = new List<string>();
        IEnumerable<string> oldDisconnectedUsers = new List<string>();
        public ChatClientRpc()
        {
            timer = new Timer(UpdateMessages, null, INTERVAL, -1);
            userTimer = new Timer(UpdateUsers, null, INTERVAL, -1);
            dcTimer = new Timer(UpdateDisconnected, null, INTERVAL, -1);
            oldMessages.ToList().Add(new ChatMessage("1", "1"));
            oldUsers.ToList().Add("1");
            oldDisconnectedUsers.ToList().Add("1");
        }

        private async void UpdateMessages(object state)
        {
            if (Interlocked.CompareExchange(ref updating, 1, 0) == 0)
            {
                try
                {
                    messages.ToList().Clear();
                    timer.Change(-1, -1);
                    messages = await GetMessagesAsync();
                    if (firstConnection)
                    {
                        RecievedMessages?.Invoke(this, new RecievedMessagesEventArgs() { Messages = messages });
                    }
                    if (messages.Count() != oldMessages.Count() && !firstConnection)
                    {

                        RecievedMessages?.Invoke(this, new RecievedMessagesEventArgs() { Messages = messages.TakeLast(1) });
                    }
                    firstConnection = false;
                    oldMessages = messages;
                    timer.Change(INTERVAL, -1);
                }
                catch (RpcException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Interlocked.Exchange(ref updating, 0);
                }
            }
        }

        private async void UpdateUsers(object state)
        {
            if (Interlocked.CompareExchange(ref userUpdating, 1, 0) == 0)
            {
                try
                {
                    users.ToList().Clear();
                    userTimer.Change(-1, -1);
                    users = await GetUsersAsync();
                    if (firstUserConnection)
                    {
                        RecievedUsers?.Invoke(this, new RecievedUsersEventArgs() { Users = users });
                    }
                    if (users.Count() != oldUsers.Count() && !firstUserConnection)
                    {
                        RecievedUsers?.Invoke(this, new RecievedUsersEventArgs() { Users = users.TakeLast(1) });
                    }
                    firstUserConnection = false;
                    oldUsers = users;
                    userTimer.Change(INTERVAL, -1);
                }
                catch (RpcException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Interlocked.Exchange(ref userUpdating, 0);
                }
            }
        }

        private async void UpdateDisconnected(object state)
        {
            if (Interlocked.CompareExchange(ref dcUpdating, 1, 0) == 0)
            {
                try
                {
                    disconnectedUsers.ToList().Clear();
                    dcTimer.Change(-1, -1);
                    disconnectedUsers = await GetDisconnectedUsersAsync();
                    if (firstDcConnection)
                    {
                        LeftUsers?.Invoke(this, new LeftUserEventArgs() { DisconnectedUsers = disconnectedUsers });
                    }
                    if (disconnectedUsers.Count() != oldDisconnectedUsers.Count() && !firstDcConnection)
                    {
                        LeftUsers?.Invoke(this, new LeftUserEventArgs() { DisconnectedUsers = disconnectedUsers.TakeLast(1) });
                    }
                    firstDcConnection = false;
                    oldDisconnectedUsers = disconnectedUsers;
                    dcTimer.Change(INTERVAL, -1);
                }
                catch (RpcException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Interlocked.Exchange(ref dcUpdating, 0);
                }
            }
        }

        private async Task<IEnumerable<ChatMessage>> GetMessagesAsync()
        {
            List<ChatMessage> messages = new();
            using (var stream = client.GetMessages(new GetMessagesRequest()))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var currentResponse = stream.ResponseStream.Current;
                    messages.Add(new ChatMessage(currentResponse.Text, currentResponse.From));
                }
            }
            return messages;
        }

        private async Task<IEnumerable<string>> GetUsersAsync()
        {
            List<string> users = new();
            using (var stream = client.GetUsers(new GetUserRequest()))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var currentUser = stream.ResponseStream.Current;
                    users.Add(currentUser.Username);
                }
            }
            return users;
        }

        private async Task<IEnumerable<string>> GetDisconnectedUsersAsync()
        {
            List<string> disconnected = new();
            using (var stream = client.GetDisconnectedUsers(new GetDisconnectedUsersRequest()))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var currentUser = stream.ResponseStream.Current;
                    disconnected.Add(currentUser.Username);
                }
            }
            return disconnected;
        }

        public void Refresh()
        {
            UpdateMessages(null);
        }

        public void RefreshUsers()
        {
            UpdateUsers(null);
        }

        public void RefreshDisconnected()
        {
            UpdateDisconnected(null);
        }

        public void SendMessage(string from, string text)
        {
            client.SendMessage(new SendRequest { From = from, Text = text });
        }

        public void SendUser(string username)
        {
            client.SendUser(new SendUserRequest { Username = username });
        }

        public void SendDisconnectedUser(string username)
        {
            client.SendDisconnectedUser(new SendDisconnectedRequest { Username = username });
        }

        public void ShutDown()
        {
            //TO-DO
        }
    }
}