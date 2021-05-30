using ChatService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatService.Utility
{
    public static class ChatBase
    {
        public static List<ChatMessage> ChatLog { get; set; }
        public static List<User> UserList { get; set; }
        public static Message LastMessageSent { get; set; }

        static ChatBase()
        {
            ChatLog = new List<ChatMessage>();
            //ChatLog.Add(new Message() { User = new User { Id = 0, Name = string.Empty }, Text = string.Empty });
           // LastMessageSent = ChatLog.Last();
            UserList = new List<User>();
        }

        public static void WriteToMessageList(ChatMessage message)
        {
            ChatLog.Add(message);
        }

        public static void WriteToUserList(User user)
        {
            UserList.Add(user);
        }

        public static IEnumerable<ChatMessage> GetAllMessages()
        {
            return ChatLog;
        }
    }
}