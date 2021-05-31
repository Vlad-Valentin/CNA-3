using ChatService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatService.Utility
{
    public static class ChatBase
    {
        public static List<ChatMessage> ChatLog { get; set; }
        public static List<string> UserList { get; set; }
        public static Message LastMessageSent { get; set; }

        static ChatBase()
        {
            ChatLog = new List<ChatMessage>();
            //ChatLog.Add(new Message() { User = new User { Id = 0, Name = string.Empty }, Text = string.Empty });
           // LastMessageSent = ChatLog.Last();
            UserList = new List<string>();
        }

        public static void WriteToMessageList(ChatMessage message)
        {
            ChatLog.Add(message);
        }

        public static void WriteToUserList(string username)
        {
            UserList.Add(username);
        }

        public static IEnumerable<ChatMessage> GetAllMessages()
        {
            return ChatLog;
        }

        public static IEnumerable<string> GetAllUsers()
        {
            return UserList;
        }
    }
}