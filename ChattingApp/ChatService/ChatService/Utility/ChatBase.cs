using System.Collections.Generic;
using System.Text;

namespace ChatService.Utility
{
    public static class ChatBase
    {
        public static List<Message> ChatLog { get; set; }
        public static List<User> UserList { get; set; }

        static ChatBase()
        {
            ChatLog = new List<Message>();
            UserList = new List<User>();
        }

        public static void WriteToMessageList(Message message)
        {
            ChatLog.Add(message);
        }

        public static void WriteToUserList(User user)
        {
            UserList.Add(user);
        }
    }
}