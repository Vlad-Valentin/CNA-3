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
    public class ChatClientRpc
    {
        public const int INTERVAL = 1000;

        public bool firstConnection = true;

        public delegate void MessagesRecievedHandler(object Sender, RecievedMessagesEventArgs args);
        public event MessagesRecievedHandler RecievedMessages;
        private int updating = 0;

        ChatClient.MessengerService.MessengerServiceClient client = new GrpcServiceProvider().GetMessengerClient();
        Timer timer;

        IEnumerable<ChatMessage> messages = new List<ChatMessage>();
        IEnumerable<ChatMessage> oldMessages = new List<ChatMessage>();
        public ChatClientRpc()
        {
            timer = new Timer(UpdateMessages, null, INTERVAL, -1);
            oldMessages.ToList().Add(new ChatMessage("1", "1"));
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
                        firstConnection = false;
                    }
                    if (messages.Count() != oldMessages.Count())
                    {

                        RecievedMessages?.Invoke(this, new RecievedMessagesEventArgs() { Messages = messages.TakeLast(1) });
                    }
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

        public void Refresh()
        {
            UpdateMessages(null);
        }

        public void SendMessage(string from, string text)
        {
            client.SendMessage(new SendRequest { From = from, Text = text });
        }

        public void ShutDown()
        {
            //TO-DO
        }
    }
}