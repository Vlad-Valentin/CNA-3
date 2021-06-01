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

        public static List<string> DisconnectedUserList { get; set; }

        static ChatBase()
        {
            ChatLog = new List<ChatMessage>();
            UserList = new List<string>();
            DisconnectedUserList = new List<string>();
        }

        public static void WriteToMessageList(ChatMessage message)
        {
            ChatLog.Add(message);
        }

        public static void WriteToUserList(string username)
        {
            UserList.Add(username);
        }

        public static void WriteToDisconnectedUserList(string username)
        {
            DisconnectedUserList.Add(username);
        }

        public static IEnumerable<ChatMessage> GetAllMessages()
        {
            return ChatLog;
        }

        public static IEnumerable<string> GetAllUsers()
        {
            return UserList;
        }

        public static IEnumerable<string> GetAllDisconnectedUsers()
        {
            return DisconnectedUserList;
        }
    }
}