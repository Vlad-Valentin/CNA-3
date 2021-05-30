using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using ChatService.Models;

namespace ChatService.Utility
{
    public class ChatServiceHelper
    {
        private event Action<ChatMessage> AddedEvent;

        public void Add(ChatMessage message)
        {
            ChatBase.WriteToMessageList(message);
            AddedEvent?.Invoke(message);
        }

        public IObservable<Message> GetLogAsObservable()
        {
            var previous = ChatBase.GetAllMessages().ToObservable();
            var newChat = Observable.FromEvent<ChatMessage>((msg) => AddedEvent += msg, (msg) => AddedEvent -= msg);
            // return previous.Concat(newChat);
            return null;
        }
    }
}
